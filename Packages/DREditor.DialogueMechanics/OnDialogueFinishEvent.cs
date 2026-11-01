//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Finishing;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IFinishableDialogueComponent) +
        "\n\n")]
    public class OnDialogueFinishEvent : DialogueComponent, IFinishableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueEventString = "CheckFocus";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float finishOrder = 4;


        public float Order { get => finishOrder; }


        /// <inheritdoc/>
        public virtual void OnFinishDialogue() => DialogueEventSystem.TriggerEvent(dialogueEventString);
    }
}