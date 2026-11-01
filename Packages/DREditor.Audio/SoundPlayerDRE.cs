//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class SoundPlayerDRE : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        public abstract void PlayMusic();

        /// <summary>
        ///
        /// </summary>
        public abstract void PlaySFX();

        /// <summary>
        ///
        /// </summary>
        public abstract void PlayVoiceLine();
    }
}