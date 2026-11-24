//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using System;

namespace DREditor.EndEvents
{
    /// <summary>
    /// 
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) +
        "\n\n")]
    public class DialogueEventEndEventHandler : DialogueComponent, IDialogueEndEvent
    {
        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue) where T : Line
        {
            if (currentDialogue is IEndEventCollection container)
            {
                var dialogueEndEvent = container.GetEndEvent<DialogueEventEndEvent>();

                foreach (var dialogueEvent in dialogueEndEvent.DialogueEvents)
                    dialogueEvent.TriggerDialogueEvent();
            }

            return false;
        }
    }
}