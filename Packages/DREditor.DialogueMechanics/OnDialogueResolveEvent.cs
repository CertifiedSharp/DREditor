//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IResolvableDialogueComponent) +
        "\n\n")]
    public class OnDialogueResolveEvent : DialogueComponent, IResolvableDialogueComponent
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
        [SerializeField] protected float resolveOrder = 0;


        public float Order { get => resolveOrder; }


        /// <inheritdoc/>
        public virtual void Resolve() => DialogueEventSystem.TriggerEvent(dialogueEventString);

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            Resolve();

            yield break;
        }

        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => false;
    }
}