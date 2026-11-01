//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Progressing.Handling;
using DREditor.Services;
using System;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) +
        "\n\n")]
    public class TriggerFlagEndEventHandler : DialogueComponent, IDialogueEndEvent
    {
        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue) 
            where T : Line
        {
            TriggerDialogueFlag(currentDialogue);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        protected virtual void TriggerDialogueFlag<T>(DialogueBase<T> currentDialogue)
        {
            if (currentDialogue.FlagTrigger != null && currentDialogue.FlagTrigger.Enabled)
            {
                TriggerFlag triggerFlag = currentDialogue.FlagTrigger;

                ServiceLocator.GetService<FlagTriggerHandlerBase>()
                    .TriggerFlag(triggerFlag.Chapter, triggerFlag.Objective, triggerFlag.Flags);
            }
        }
    }
}