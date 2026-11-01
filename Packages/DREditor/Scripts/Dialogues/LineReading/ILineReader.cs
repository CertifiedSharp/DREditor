//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    public interface ILineReader
    {
        /// <summary>
        ///
        /// </summary>
        event Action OnLineStart;

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        event Action OnLineEndEvent;
    }
}