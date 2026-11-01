//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.Finishing
{
    /// <summary>
    ///
    /// </summary>
    public interface IEndDialogueEvent
    {
        /// <summary>
        ///
        /// </summary>
        event Action OnDialogueEnd;
    }
}