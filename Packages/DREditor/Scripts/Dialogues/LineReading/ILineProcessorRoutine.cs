//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    public interface ILineProcessorRoutine : ILineRoutine
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        IEnumerator LineProcessRoutine(Line currentLine);
    }
}