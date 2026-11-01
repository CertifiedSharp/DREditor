//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<TPFDRoomLoader>) + " for type: " + nameof(TPFDRoomLoader) +
        "\n\n")]
    public class TPFDRoomLoader : ServiceHolder<TPFDRoomLoader>
    {
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
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inThreeDMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureFreezeFrameEventString = "CaptureMain";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string toTPFDFromDialogueEventString = "ToTPFDFromDialogue";


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void SetMovingToTPFDMode()
        {
            playerSpawnPoint.SetValue(new PosRot(Vector3.zero, Quaternion.identity));

            inTPFD.AddListener(ToTPFDRoom);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ToTPFDRoom(bool to)
        {
            inTPFD.RemoveListener(ToTPFDRoom);

            if (gateDatabase.GetArea(SceneManager.GetActiveScene().name).isTPFD && !inTPFD.Value)
                inTPFD.SetValue(true);

            if (inThreeDMode.Value && inDialogue.Value)
                DialogueEventSystem.TriggerEvent(captureFreezeFrameEventString, true);

            if (!inTPFD.Value || (inThreeDMode.Value && inDialogue.Value))
                return;
            else
                DialogueEventSystem.TriggerEvent(toTPFDFromDialogueEventString);
        }
    }
}