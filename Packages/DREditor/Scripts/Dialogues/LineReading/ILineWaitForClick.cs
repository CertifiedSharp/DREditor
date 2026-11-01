//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    public interface ILineWaitForClick
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="currentLine"></param>
        /// <param name="exitParentCallback"></param>
        IEnumerator LineWaitForClick<T>(DialogueBase<T> currentDialogue, Line currentLine, Action exitParentCallback = null)
            where T : Line;

        /// <summary>
        ///
        /// </summary>
        bool SkipOnFastForward { get; }
    }
}