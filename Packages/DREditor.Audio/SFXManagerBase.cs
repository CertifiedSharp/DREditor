//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class SFXManagerBase : SoundService<SFXManagerBase>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        public abstract void PlaySFX<T>(T eventName);

        /// <summary>
        ///
        /// </summary>
        public abstract void StopSoundFX();

        /// <summary>
        ///
        /// </summary>
        public abstract void PlayCancel();

        /// <summary>
        ///
        /// </summary>
        public abstract void PlaySelect();

        /// <summary>
        ///
        /// </summary>
        public abstract void PlaySubmit();
    }
}