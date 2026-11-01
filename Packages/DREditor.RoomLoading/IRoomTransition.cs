//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    public interface IRoomTransition
    {
        /// <summary>
        ///
        /// </summary>
        bool IsTransitioning { get; }


        /// <summary>
        ///
        /// </summary>
        void StartTransition();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator StartTransitionRoutine();

        /// <summary>
        ///
        /// </summary>
        void EndTransition();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator EndTransitionRoutine();
    }
}