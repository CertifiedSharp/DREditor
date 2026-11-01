//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public interface IEarlyResolvableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator ResolveEarly();
    }
}