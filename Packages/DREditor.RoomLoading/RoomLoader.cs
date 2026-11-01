//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<RoomLoaderBase>) + " for type: " + nameof(RoomLoaderBase) +
        "\n\n")]
    public class RoomLoader : RoomLoaderBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onSetPlayerState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent setControlsUIBasedOnCurrentMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] bool debugLoadRoomOnStart = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<RoomLoadComponent> roomLoadRoutines = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<RoomLoadComponent> roomLoadResolvers = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingToRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent skipActivateControls = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float setPlayerPositionBufferTime = 0.1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string enableMonoControlsEvent = "EnableControlMonobehaviour";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string disableControlsEvent = "DisableAbility";


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (debugLoadRoomOnStart)
                LoadRoom(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        /// <inheritdoc/>
        public override void AllowRoomsToLoad() => SceneManager.sceneLoaded += LoadRoom;

        /// <inheritdoc/>
        public override void DoNotAllowRoomsToLoad() => SceneManager.sceneLoaded -= LoadRoom;

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected virtual void LoadRoom(Scene scene, LoadSceneMode mode) => StartCoroutine(LoadIntoRoom(scene));

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        protected virtual IEnumerator LoadIntoRoom(Scene scene)
        {
            if (AlreadyLoadingRoom(scene.name))
                yield break;

            isLoadingToRoom.SetValue(true);

            onLoadingRoom.Raise();

            DialogueEventSystem.TriggerEvent(disableControlsEvent);

            onSetPlayerState.Raise();

            yield return new WaitForSeconds(setPlayerPositionBufferTime);

            yield return OnRoomLoadRoutines();

            yield return DisplayEndTransitions();

            OnRoomLoadResolvers();

            EnableControlsFinishLoadingFile();

            FinishRoomLoadingEvents();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        protected virtual bool AlreadyLoadingRoom(string sceneName)
        {
            if (isLoadingToRoom.Value)
                UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}: The game is already fading into the desired room.");
            else
                UnityEngine.Debug.Log($"REPORT: {gameObject.name}: Scene: " + sceneName + " is now Loading");

            return isLoadingToRoom.Value;
        }
        
        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplayEndTransitions()
        {
            bool notLoadingIntoTrialOrDialogueFile = !isLoadingFile.Value && !inTrialMode.Value && !inDialogue.Value;

            if (notLoadingIntoTrialOrDialogueFile && !ServiceLocator.GetService<GlobalFade>().IsDark)
                yield return ServiceLocator.GetService<IRoomTransition>().EndTransitionRoutine();

            yield return DisplayPostTransitions();

            onFinishedRoomTransitions.Raise();

            UnityEngine.Debug.Log($"REPORT: {gameObject.name}: Room Transitions finished, finishing up loading.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplayPostTransitions()
        {
            if (FindObjectsByType<MonoBehaviour>().OfType<IPostTransitionRoutine>() is IPostTransitionRoutine[] result)
                foreach (IPostTransitionRoutine postTransition in result)
                    yield return postTransition.PostTransitionRoutine();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableControlsFinishLoadingFile()
        {
            if (!inTrialMode.Value && !skipActivateControls.Value)
                DialogueEventSystem.TriggerEvent(enableMonoControlsEvent);

            if (isLoadingFile.Value)
                isLoadingFile.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator OnRoomLoadRoutines()
        {
            foreach (RoomLoadComponent roomLoadComponent in roomLoadRoutines)
                if (roomLoadComponent is IRoomLoadRoutine roomLoadRoutine)
                    yield return roomLoadRoutine.OnRoomLoadRoutine();
                else
                    UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}: " +
                        $"{roomLoadComponent.name} does not inherit the specified interface.");

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRoomLoadResolvers()
        {
            foreach (RoomLoadComponent roomLoadComponent in roomLoadResolvers)
                if (roomLoadComponent is IRoomLoadResolver roomLoadResolver)
                    roomLoadResolver.OnRoomLoadResolve();
                else
                    UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}: " +
                        $"{roomLoadComponent.name} does not inherit the specified interface.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FinishRoomLoadingEvents()
        {
            UnityEngine.Debug.Log($"REPORT: {gameObject.name}: Finished loading room.");

            onFinishedLoadingRoom.Raise();

            if (!inDialogue.Value)
            {
                onFinishedLoadingRoomNoDialogue.Raise();

                setControlsUIBasedOnCurrentMode.Raise();
            }
            else
                UnityEngine.Debug.Log($"REPORT: {gameObject.name}: End of RoomLoader with In Dialogue True.");
        }
    }
}