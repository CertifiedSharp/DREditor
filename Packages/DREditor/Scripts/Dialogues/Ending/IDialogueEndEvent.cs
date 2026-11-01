//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueEndEvent
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="interruptDialogue"></param>
        /// <returns></returns>
        bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line;
    }
}