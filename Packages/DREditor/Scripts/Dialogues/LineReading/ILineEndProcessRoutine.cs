//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    public interface ILineEndProcessRoutine
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        IEnumerator LineEndProcess(Line currentLine = null);
    }
}