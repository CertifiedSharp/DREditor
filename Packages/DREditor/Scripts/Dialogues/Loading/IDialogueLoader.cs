//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueLoader
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueData"></param>
        IEnumerator LoadDialogueProcess(object dialogueData = null);
    }
}