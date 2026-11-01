//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class NotLeavingOnDialogueFinish : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueFinisher DialogueFinisher = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLeaving = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => DialogueFinisher.SubscribeToOnFinishOfDialogue(TurnInLeavingModeOff);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueFinisher.UnsubscribeToOnFinishOfDialogue(TurnInLeavingModeOff);

        /// <summary>
        ///
        /// </summary>
        protected virtual void TurnInLeavingModeOff() => isLeaving.SetValue(false);

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => TurnInLeavingModeOff();
    }
}