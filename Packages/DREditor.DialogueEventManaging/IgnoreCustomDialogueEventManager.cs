//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventIgnorer) + ", " + nameof(ILineStarter) +
        "\n\n")]
    public class IgnoreCustomDialogueEventManager : DialogueEventIgnorer, ILineStarter
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> omittedCustomEvents = new();


        /// <inheritdoc/>
        public virtual void LineStartProcess(Line currentLine) => InvokeCustomLineEvents(currentLine);


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void InvokeCustomLineEvents(Line currentLine)
        {
            if (currentLine.DiaEvents.OfType<CustomEvent>().ToList()
                is List<CustomEvent> customs && customs.Count > 0)
            {
                customs = customs.OrderBy(n => n.ExecutionOrder).ToList();

                foreach (CustomEvent customEvent in customs)
                    if (omittedCustomEvents.Contains(customEvent.EventName))
                        AddIgnoredEvent(customEvent);
                    else if (customEvent.ExecutionOrder > 0)
                        InvokeIgnorableEvent(customEvent);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customEvent"></param>
        protected virtual void InvokeIgnorableEvent(IDialogueEvent customEvent)
        {
            AddIgnoredEvent(customEvent);

            customEvent.TriggerDialogueEvent();
        }

        /// <inheritdoc/>
        public override bool IsIgnoredEvent(IDialogueEvent dialogueEvent)
        {
            if (base.IsIgnoredEvent(dialogueEvent))
                return true;

            foreach (var omittedEvent in omittedCustomEvents)
                if (dialogueEvent is CustomEvent customEvent && customEvent.EventName == omittedEvent)
                    return true;

            return false;
        }

        [ContextMenu("Apply Default Events")]
        public void ApplyDefaultEvents()
        {
            omittedCustomEvents.Add("FlashDialogueScreenWhite");

            omittedCustomEvents.Add("ActorFade");

            omittedCustomEvents.Add("SpriteFlash");

            omittedCustomEvents.Add("ProtagFlash");
        }
    }
}