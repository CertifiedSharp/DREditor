//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhraseManager
    {
        /// <summary>
        ///
        /// </summary>
        UnityEvent<Panel, PhraseTextBase, float> OnPhraseInitialized { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        float PlayPhrases(Panel panel);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<PhraseTextBase> GetActivePhrases();
    }
}