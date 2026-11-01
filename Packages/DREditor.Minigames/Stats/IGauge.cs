//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    public interface IGauge
    {
        /// <summary>
        ///
        /// </summary>
        float Current { get; }

        /// <summary>
        ///
        /// </summary>
        bool IsCharged { get; }

        /// <summary>
        ///
        /// </summary>
        bool IsEmpty { get; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="deltaTime"></param>
        void Tick(float deltaTime);

        /// <summary>
        ///
        /// </summary>
        void Reset();
    }
}