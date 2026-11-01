//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueEndEventRoutine
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="exitParentEarly"></param>
        IEnumerator EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action exitParentEarly) 
            where T : Line;
    }
}