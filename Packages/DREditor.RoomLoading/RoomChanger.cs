//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Gates;
using DREditor.Progressing.Handling;
using DREditor.Services;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<RoomChangerBase>) + " for type: " + nameof(RoomChangerBase) +
        "\n\n")]
    public class RoomChanger : RoomChangerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLoading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent objectiveKeepsInstanceData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent moveToNextNaturalObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent requiredFlagsTriggered = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentObjectiveName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float hideUIWaitTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToBlackOptionTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int investigationStateIndex = 2;


        /// <inheritdoc/>
        public override void ChangeRoom(Gate destination, ScriptableObject options = null)
            => StartCoroutine(ChangeRoomRoutine(destination, (RoomLoadOptionsAsset)options));

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="options"></param>
        protected virtual IEnumerator ChangeRoomRoutine(Gate destination, RoomLoadOptionsAsset options = null)
        {
            UnityEngine.Debug.Log($"REPORT: {ToString()}: Starting room change.");

            this.options = options;

            onStartedChangingRoom.Raise();

            EventSystem.current.SetSelectedGameObject(null);

            SetDestination(destination);

            CheckChangeObjective();

            yield return new WaitForSecondsRealtime(hideUIWaitTime);

            inLoading.SetValue(true); // At this point the changing room process has started.

            yield return EvaluateStartTransition();

            AssignDestinationPosition();

            yield return ChangeRoomScene();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckChangeObjective()
        {
            Gate progressionGate = (Gate)ServiceLocator.GetService<ProgressionManagerBase>().currentObjective.ProgressionGate;

            if (requiredFlagsTriggered.Value && (progressionGate == null || progressionGate == destination))
                ChangeObjectiveCheckInstanceData();
            else
                SaveInstanceOfLeavingRoom();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeObjectiveCheckInstanceData()
        {
            UnityEngine.Debug.Log($"REPORT: {ToString()}: Game has recognized to change the objective.");

            if (!objectiveKeepsInstanceData.Value)
                ServiceLocator.GetService<InstanceManagerBase>().ClearRoomData();
            else
                SaveInstanceOfLeavingRoom();

            moveToNextNaturalObjective.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SaveInstanceOfLeavingRoom()
        {
            try
            {
                if (ServiceLocator.HasService<InstanceManagerBase>())
                    ServiceLocator.GetService<InstanceManagerBase>().SaveRoomInstance(SceneManager.GetActiveScene().name);
                else
                    UnityEngine.Debug.LogWarning($"ALERT: {ToString()}: There is no RoomInstanceManager in the scene.");
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning($"ALERT: {ToString()}: The Room could not be saved because an error occurred" +
                    $" saving the room instance.");

                UnityEngine.Debug.LogWarning(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EvaluateStartTransition()
        {
            bool abscentOptionsOrBlackTransition = options == null || !options.blackTransition;

            if (abscentOptionsOrBlackTransition)
                yield return ServiceLocator.GetService<IRoomTransition>().StartTransitionRoutine();
            else if (options != null && options.blackTransition)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeToBlackOptionTime);
        }

        /// <summary>
        ///
        /// </summary>
        public override void AssignDestinationPosition()
        {
            if (options != null && options.toWaypoint > -1 && options.toWaypoint < destination.fastTravelMarkers.Count)
                SetSpawnAtWaypoint();
            else
                SetSpawnAtFromGate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetSpawnAtWaypoint()
        {
            Waypoint waypoint = destination.fastTravelMarkers[options.toWaypoint];

            SetSpawnPositionAndRotation(waypoint.position, waypoint.rotation);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetSpawnAtFromGate()
        {
            GateFrom fromG = destination.GetFromLocation(SceneManager.GetActiveScene().name);

            if (fromG != null)
                SetSpawnPositionAndRotation(fromG.position, fromG.rotation);
            else
                SetSpawnPositionAndRotation(Vector3.zero, Vector3.zero);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        protected virtual void SetSpawnPositionAndRotation(Vector3 position, Vector3 rotation)
            => playerSpawnPoint.SetValue(new(position, Quaternion.Euler(rotation)));

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ChangeRoomScene()
        {
            string destinationSceneName = EvaluateDestinationScene();

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(destinationSceneName);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: CHANGING SCENE TO: " + destinationSceneName);

            while (!asyncOperation.isDone)
            {
                UnityEngine.Debug.Log($"REPORT: {ToString()}: Load progress: " + asyncOperation.progress);

                yield return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string EvaluateDestinationScene()
        {
            if ((options != null && options.toInvestigation) || currentState.Value == investigationStateIndex)
                return destination.GetInvestigationRoom(currentChapterIndex.Value);
            else if (destination.GetVariantScene(currentChapterIndex.Value, currentObjectiveName.Value) is string validScene)
                return validScene;
            else
                return destination.ToAreaName;
        }
    }
}