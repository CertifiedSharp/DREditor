//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.Input;
using DREditor.Services;
using DREditor.UI;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<ITeleporter>) + " for type: " + nameof(ITeleporter) +
        "\n\n")]
    public class TeleportRoomManager : ServiceHolder<ITeleporter>, ITeleporter
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inPause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("GDB")]
        [SerializeField] protected GateDatabase gateDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition teleportCondition = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RoomLoadOptionsAsset teleportOptions = null;


        /// <summary>
        ///
        /// </summary>
        protected Gate teleportGate;


        /// <inheritdoc/>
        public virtual bool RequestTeleport(string sceneName, int fastTravelIndex)
        {
            teleportOptions.SetWaypoint(fastTravelIndex);

            teleportGate = gateDatabase.GetArea(sceneName);

            bool playerCanTeleport = CanFastTravel();

            if (playerCanTeleport)
                OnRequestSuccessful();
            else
                OnRequestFailed();

            return playerCanTeleport;
        }

        /// <inheritdoc/>
        public virtual bool CanFastTravel() =>
            teleportGate != null && !ServiceLocator.GetService<MenuState>().Changing &&
            (teleportCondition == null || teleportCondition.Evaluate());

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRequestSuccessful() => StartCoroutine(ResolveThenTeleport());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveThenTeleport()
        {
            ServiceLocator.GetService<IInputManager>().Disable();

            inLeaveProcess.SetValue(true);

            DialogueEventSystem.TriggerEvent("DisableAbility");

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Waiting for game to leave pause menu.");

            yield return new WaitUntil(() => !inPause.Value);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Game Finished leaving pause menu, teleporting...");

            ServiceLocator.GetService<RoomChangerBase>().ChangeRoom(teleportGate, teleportOptions);

            ServiceLocator.GetService<IInputManager>().Enable();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRequestFailed() => UnityEngine.Debug.Log($"REPORT: {gameObject.name}: CHANGE ROOM REQUEST FAILED");
    }
}