//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class EnvironmentSFXManangerBase : SoundService<EnvironmentSFXManangerBase>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        public abstract void PlayEnv<T>(T eventName);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public abstract void StopEnv();
    }
}