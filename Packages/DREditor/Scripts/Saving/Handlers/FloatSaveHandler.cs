//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public class FloatSaveHandler : ValueEventHandler<float, FloatEvent>
    {
        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Populate Default Data")]
        public virtual void PopulateDefaultData()
        {
            entries.Add(new("BGMVolume"));

            entries.Add(new("SFXVolume"));

            entries.Add(new("VoiceVolume"));

            entries.Add(new("TextSpeed"));

            entries.Add(new("ReticleSpeed"));
        }
    }
}