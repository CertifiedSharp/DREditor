//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface ILineLastingEvent
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="continueLineCallback"></param>
        /// <returns></returns>
        IEnumerator LineLastingEvaluation(Line currentLine, Action continueLineCallback = null);
    }
}