//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueInteruptable
    {
        /// <summary>
        ///
        /// </summary>
        event Action<DialogueCore[]> OnInteruptWithNewDialogue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogues"></param>
        void DialogueInterruptedInvoke(DialogueCore[] dialogues);
    }
}