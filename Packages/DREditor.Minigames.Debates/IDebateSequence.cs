//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections.Generic;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IDebateSequence
    {
        /// <summary>
        ///
        /// </summary>
        DialogueCore DefaultRetreatDialogue { get; set; }

        /// <summary>
        ///
        /// </summary>
        List<int> PanelCharacterNumbers { get; }
    }
}