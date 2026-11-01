//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using System.Linq;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueEventManager : DialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="events"></param>
        protected virtual void TriggerDialogueEvents(IEnumerable<IDialogueEvent> events)
        {
            foreach (IDialogueEvent dialogueEvent in events)
                dialogueEvent.TriggerDialogueEvent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentLine"></param>
        protected virtual void TriggerAllDialogueEventsOfType<T>(Line currentLine) where T : IDialogueEvent
        {
            var events = currentLine.DiaEvents.Where(dialogueEvent => dialogueEvent.GetType() == typeof(T));

            if (events.Count() > 0)
                foreach (IDialogueEvent dialogueEvent in events)
                    dialogueEvent.TriggerDialogueEvent();
        }
    }
}