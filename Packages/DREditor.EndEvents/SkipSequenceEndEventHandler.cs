//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
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
    public class SkipSequenceEndEventHandler : DialogueComponent, IDialogueEndEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent skipTrialSequences = null;


        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
        {
            if (!skipTrialSequences)
                UnityEngine.Debug.LogError($"ALERT: The event field: {nameof(skipTrialSequences)} must be assigned!");
            else if (currentDialogue is ITrialSequenceSkippable skippable)
                skipTrialSequences.SetValue(skippable.Skip);

            return false;
        }
    }
}