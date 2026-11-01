//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.Starting
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueStartEvent
    {
        /// <summary>
        ///
        /// </summary>
        event Action<DialogueCore> OnDialogueStarted;
    }
}