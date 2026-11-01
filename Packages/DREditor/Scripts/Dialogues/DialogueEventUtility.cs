//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Dialogues.Events;
using System;
using System.Collections;
using System.Linq;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class DialogueEventUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentLine"></param>
        /// <param name="onEventFound"></param>
        public static void ContainsDialogueEvent<T>(Line currentLine, Action<T> onEventFound)
            where T : IDialogueEvent
        {
            if (currentLine.DiaEvents.FirstOrDefault(n => n is T) is T dialogueEvent)
                onEventFound?.Invoke(dialogueEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentLine"></param>
        /// <param name="onEventFound"></param>
        /// <returns></returns>
        public static IEnumerator TriggerDialogueEventRoutine<T>(Line currentLine, Func<T, IEnumerator> onEventFound)
            where T : IDialogueEvent
        {
            if (currentLine.DiaEvents.FirstOrDefault(n => n is T) is T dialogueEvent)
                yield return onEventFound(dialogueEvent);

            yield break;
        }
    }
}