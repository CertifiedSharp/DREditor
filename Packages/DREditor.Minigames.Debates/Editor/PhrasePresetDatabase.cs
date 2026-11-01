//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Minigames/Phrases/Phrase Preset Database", fileName = "PhrasePresetDatabase")]
    public class PhrasePresetDatabase : ScriptableObject, IDebateConverter
    {
        /// <summary>
        ///
        /// </summary>
        public List<PhrasePreset> Presets = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetNames() => Presets.Select(preset => preset.presetName).ToArray();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int[] GetInts() => Enumerable.Range(0, Presets.Count).ToArray();


        /// <inheritdoc/>
        public virtual void Convert()
        {
            foreach (var preset in Presets)
                preset.preset.Convert();
        }
    }
}