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
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) + ", " + nameof(IPhaseChangeEvent<StartInvestigation>) +
        "\n\n")]
    public class PhaseChangeEndEventHandler : DialogueComponent, IDialogueEndEvent, IPhaseChangeEvent<StartInvestigation>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent cleanUpDialogueReader = null;


        public event Action<StartInvestigation> PhaseChanged;


        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue) 
            where T : Line
            => EvaluateEndPhaseChangeEvent(currentDialogue);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual bool EvaluateEndPhaseChangeEvent<T>(DialogueBase<T> dialogue) 
            where T : Line
        {
            if (dialogue is Dialogue currentDialogue && currentDialogue != null)
            {
                if (currentDialogue.StartInvestigation.Enabled)
                {
                    cleanUpDialogueReader.Raise();

                    PhaseChanged?.Invoke(currentDialogue.StartInvestigation);
                }

                return currentDialogue.StartInvestigation.Enabled;
            }

            return false;
        }
    }
}