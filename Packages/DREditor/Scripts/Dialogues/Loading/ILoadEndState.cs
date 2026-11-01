//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface ILoadEndState
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        IEnumerator LoadEndStates(Line currentLine = null);
    }
}