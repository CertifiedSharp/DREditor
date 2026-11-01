//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IFinishableDialogueComponent) +
            "\n\n")]
    public class MoveEndOnDialogueFinish : DialogueComponent, IFinishableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isGoingToMainMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string setMoveGateEventString = "MoveEndOnDialogueFinish";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string disableControlsEventString = "DisableAbility";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float finishOrder = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int finisherID = 1;


        public float Order { get => finishOrder; }

        public int ExclusiveHandlerID => finisherID;


        /// <summary>
        ///
        /// </summary>
        protected Gate gateToMove = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            DialogueEventSystem.StartListening(setMoveGateEventString, SetGateToMoveTo);

            SceneManager.sceneLoaded += ClearGateOnSceneLoad;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            DialogueEventSystem.StopListening(setMoveGateEventString, SetGateToMoveTo);

            SceneManager.sceneLoaded -= ClearGateOnSceneLoad;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public virtual void SetGateToMoveTo(object o = null) => gateToMove = (Gate)o;


        /// <inheritdoc/>
        public virtual void OnFinishDialogue() => CheckMoveOnFinish();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckMoveOnFinish()
        {
            if (gateToMove != null && !inLeaveProcess.Value)
                MoveToRoom();

            isGoingToMainMenu.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToRoom()
        {
            inLeaveProcess.SetValue(true);

            DialogueEventSystem.TriggerEvent(disableControlsEventString);

            ServiceLocator.GetService<RoomChangerBase>().ChangeRoom(gateToMove);

            gateToMove = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected virtual void ClearGateOnSceneLoad(Scene scene, LoadSceneMode mode) => gateToMove = null;
    }
}