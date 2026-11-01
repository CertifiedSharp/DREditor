//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Gates;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class RoomLoaderDebugger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onSetPlayerState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedRoomTransitions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedLoadingRoomNoDialogue = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] bool debugTestOnLoad = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] bool debugMoveArea = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] bool debugLoadPositionFromGate = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] Gate debugToGate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] Gate debugFromGate = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RoomChangerBase roomChanger = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RoomLoaderBase roomLoader = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => DebugRoomLoader();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DebugRoomLoader()
        {
            if (debugMoveArea || debugTestOnLoad || debugLoadPositionFromGate)
                roomLoader.AllowRoomsToLoad();

            if (debugTestOnLoad)
                DebugTestOnLoad();

            if (debugLoadPositionFromGate)
                DebugLoadPositionFromGate(debugToGate, debugFromGate);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="debugToGate"></param>
        /// <param name="debugFromGate"></param>
        protected virtual void DebugLoadPositionFromGate(Gate debugToGate, Gate debugFromGate)
        {
            if (debugToGate != null && debugFromGate != null)
                roomChanger.SetDestination(debugToGate);
            else
                UnityEngine.Debug.LogError($"ALERT: ERROR: {ToString()}: Debugging when DebugToGate or FromGate hasn't been filled!");

            GateFrom fromG = debugToGate.GetFromLocation(debugFromGate);

            if (fromG != null)
                SetSpawnPositionAndRotation(fromG.position, fromG.rotation);
            else
                SetSpawnPositionAndRotation(Vector3.zero, Vector3.zero);

            onSetPlayerState.Raise();
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
        protected virtual void DebugTestOnLoad()
        {
            UnityEngine.Debug.LogWarning($"ALERT: {ToString()}: DebugTestOnLoad is ON in debugging. This should only be on to test" +
                    " OnLoad Events/Triggers, otherwise it should be turned OFF. \n DO NOT USE THIS TO TEST " +
                    "AN ROOM SAVE, THE ONLOAD CALLS ARE CALLED ON START WHEN THIS IS ACTIVE.");

            onFinishedRoomTransitions.Raise();

            onFinishedLoadingRoomNoDialogue.Raise();
        }
    }
}