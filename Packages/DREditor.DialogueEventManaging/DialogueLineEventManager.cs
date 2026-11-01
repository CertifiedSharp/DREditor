//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FilteredDialogueEventManager) + ", " + nameof(ILineProcessorRoutine) + ", " + nameof(ILoadToLineEvents) +
        "\n\n")]
    public class DialogueLineEventManager : FilteredDialogueEventManager, ILineProcessorRoutine, ILoadToLineEvents
    {
        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine)
        {
            bool hasEvents = currentLine.DiaEvents.Count != 0;

            bool hasEventWaiters = LineContainsEventWaiters(currentLine, out _);

            if (hasEvents && !hasEventWaiters)
                TriggerDialogueLineEvents(currentLine);

            return hasEvents && hasEventWaiters;
        }

        /// <inheritdoc/>
        public IEnumerator LineProcessRoutine(Line currentLine) => EvaluateDialogueEvents(currentLine);


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual IEnumerator EvaluateDialogueEvents(Line currentLine)
        {
            if (currentLine.DiaEvents.Count != 0)
                if (LineContainsEventWaiters(currentLine, out List<IDialogueEvent> list))
                    yield return SplitTriggeringWaiterEvents(currentLine, list);
                else
                    TriggerDialogueLineEvents(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="dialogueEvents"></param>
        /// <returns></returns>
        protected virtual bool LineContainsEventWaiters(Line currentLine, out List<IDialogueEvent> dialogueEvents)
        {
            dialogueEvents = currentLine.DiaEvents.Where(dialogueEvent => dialogueEvent is EventWaiter).ToList();

            return dialogueEvents.Count > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="dialogueEvents"></param>
        /// <returns></returns>
        protected virtual IEnumerator SplitTriggeringWaiterEvents(Line currentLine, List<IDialogueEvent> dialogueEvents)
        {
            yield return TriggerEventWaiters(dialogueEvents);

            TriggerNonEventWaiters(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvents"></param>
        /// <returns></returns>
        protected virtual IEnumerator TriggerEventWaiters(List<IDialogueEvent> dialogueEvents)
        {
            foreach (IDialogueEvent dialogueEvent in dialogueEvents)
            {
                EventWaiter dialogueEventWaiter = dialogueEvent as EventWaiter;

                dialogueEventWaiter.SetUnfinished();

                dialogueEvent.TriggerDialogueEvent();

                UnityEngine.Debug.Log($"REPORT: {ToString()}: Started waiting for event.");

                yield return new WaitUntil(() => dialogueEventWaiter.done);

                dialogueEventWaiter.SetUnfinished();

                UnityEngine.Debug.Log($"REPORT: {ToString()}: Finished waiting for event.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void TriggerNonEventWaiters(Line currentLine)
        {
            List<IDialogueEvent> nonEventWaiterDialogueEvents = currentLine.DiaEvents
                .Where(dialogueEvent => dialogueEvent is not EventWaiter).ToList();

            if (nonEventWaiterDialogueEvents.Count > 0)
                foreach (IDialogueEvent dialogueEvent in nonEventWaiterDialogueEvents)
                    dialogueEvent.TriggerDialogueEvent();
        }


        /// <inheritdoc/>
        public virtual void OnLoadToLineEvents(Line currentLine) => TriggerDialogueLineEvents(currentLine);
    }
}