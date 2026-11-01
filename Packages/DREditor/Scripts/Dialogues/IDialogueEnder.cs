//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueEnder
    {
        /// <summary>
        ///
        /// </summary>
        event Action OnDialogueResolving;


        /// <summary>
        ///
        /// </summary>
        void StopDialogue();

        /// <summary>
        ///
        /// </summary>
        /// <param name="useDialogueFinisher"></param>
        /// <returns></returns>
        /// <remarks>
        ///
        /// </remarks>
        IEnumerator EndDialogue(bool useDialogueFinisher = true);

        /// <summary>
        ///
        /// </summary>
        void FinishDialogue();
    }
}