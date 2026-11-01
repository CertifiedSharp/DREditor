//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public class FilteredDialogueEventManager : DialogueEventManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueEventIgnorer> eventIgnorers = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueEventTypeExcluder> eventTypeExcluders = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        /// <returns></returns>
        protected virtual bool EventShouldBeTriggered(IDialogueEvent dialogueEvent) 
            => !EventTypeIsExcluded(dialogueEvent) && !EventIsToBeIgnored(dialogueEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        /// <returns></returns>
        protected virtual bool EventTypeIsExcluded(IDialogueEvent dialogueEvent)
        {
            foreach (var excluder in eventTypeExcluders)
                if (excluder != null && excluder.IsExcludedEvent(dialogueEvent))
                    return true;
                else if (excluder == null)
                    UnityEngine.Debug.LogWarning($"ALERT: {nameof(FilteredDialogueEventManager)} {gameObject.name}: " +
                        $"There is an excluder that is null, please remove it from the {eventTypeExcluders} list.");

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        /// <returns></returns>
        protected virtual bool EventIsToBeIgnored(IDialogueEvent dialogueEvent)
        {
            foreach (var ignorer in eventIgnorers)
                if (ignorer.IsIgnoredEvent(dialogueEvent))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void TriggerDialogueLineEvents(Line currentLine)
        {
            // Because custom events can be called first/handled/called separately, the game needs to filter them out.
            var events = currentLine.DiaEvents.Where(dialogueEvent => EventShouldBeTriggered(dialogueEvent));

            TriggerDialogueEvents(events);

            ClearIgnorers();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearIgnorers()
        {
            foreach (var ignorer in eventIgnorers)
                ignorer.ClearIgnoredEvents();
        }
    }
}