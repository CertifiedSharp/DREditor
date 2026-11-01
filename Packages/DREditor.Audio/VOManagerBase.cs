//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class VOManagerBase : SoundService<VOManagerBase>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        public abstract void PlayVoiceLine<T>(T eventName);

        /// <summary>
        ///
        /// </summary>
        public abstract void StopVoiceLine();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract bool VoicePlaying();
    }
}