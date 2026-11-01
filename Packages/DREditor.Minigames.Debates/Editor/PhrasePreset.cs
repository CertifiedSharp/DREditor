//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class PhrasePreset
    {
        /// <summary>
        ///
        /// </summary>
        public string presetName;

        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        public PhraseBase preset;

        public PhrasePreset(string presetName, PhraseBase preset)
        {
            this.presetName = presetName;

            this.preset = preset;
        }
    }
}