//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using System.Collections.Generic;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueEventIgnorer : DialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly List<IDialogueEvent> ignoredEvents = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoredEvent"></param>
        protected virtual void AddIgnoredEvent(IDialogueEvent ignoredEvent) => ignoredEvents.Add(ignoredEvent);


        /// <summary>
        ///
        /// </summary>
        public virtual void ClearIgnoredEvents() => ignoredEvents.Clear();

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        /// <returns></returns>
        public virtual bool IsIgnoredEvent(IDialogueEvent dialogueEvent)
        {
            foreach (IDialogueEvent i in ignoredEvents)
                if (dialogueEvent == i)
                    return true;

            return false;
        }
    }
}