//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System;
using UnityEngine;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) +
        "\n\n")]
    public class CustomCallEndEventHandler : DialogueComponent, IDialogueEndEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLeaving = null;


        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue) 
            where T : Line
            => EvaluateCustomEndEvent(currentDialogue, interruptDialogue);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <param name="interruptDialogue"></param>
        /// <returns></returns>
        protected virtual bool EvaluateCustomEndEvent<T>(DialogueBase<T> dialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
        {
            if (dialogue is Dialogue currentDialogue && currentDialogue != null)
            {
                bool isntLeaving = !isLeaving.Value;

                bool endEventIsValid = currentDialogue.EndEvent.Enabled && currentDialogue.EndEvent.CustomCall != "";

                bool shouldTriggerEvent = endEventIsValid && isntLeaving;

                if (shouldTriggerEvent)
                    DialogueEventSystem.TriggerEvent(currentDialogue.EndEvent.CustomCall);

                if (shouldTriggerEvent && currentDialogue.EndEvent.CallBreak)
                    return true;
            }

            return false;
        }
    }
}