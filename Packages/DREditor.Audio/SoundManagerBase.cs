//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SoundManagerBase : SoundService<SoundManagerBase>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected MusicManagerBase musicManager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SFXManagerBase sfxManager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected VOManagerBase voManager = null;


        /// <summary>
        ///
        /// </summary>
        public virtual void StopAllButMusic()
        {
            sfxManager.StopSoundFX();

            voManager.StopVoiceLine();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void StopAllButVO()
        {
            musicManager.StopMusic();

            sfxManager.StopSoundFX();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void StopAllAudio()
        {
            musicManager.StopSound();

            StopAllButMusic();
        }


        /// <summary>
        ///
        /// </summary>
        public abstract void MuteAudio();

        /// <summary>
        ///
        /// </summary>
        public abstract void UnmuteAudio();
    }
}