//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessor) +
        "\n\n")]
    public class TriggerCustomDialogueEventDuringLine : DialogueComponent, ILineProcessor
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueEventString = "FlashDialogueScreenWhite";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool includeEventsWithMatchingCall = false;


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            if (includeEventsWithMatchingCall)
                ProcessAllEvents(currentLine);
            else
                ProcessCustomEvents(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void ProcessAllEvents(Line currentLine)
        {
            foreach (var dialogueEvent in currentLine.DiaEvents)
                if (dialogueEvent.GetType().Name == dialogueEventString ||
                    (dialogueEvent is CustomEvent customEvent && customEvent.EventName == dialogueEventString))
                    DialogueEventSystem.TriggerEvent(dialogueEventString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void ProcessCustomEvents(Line currentLine)
        {
            foreach (CustomEvent customEvent in currentLine.DiaEvents.OfType<CustomEvent>())
                if (customEvent.EventName == dialogueEventString)
                    DialogueEventSystem.TriggerEvent(dialogueEventString);
        }
    }
}