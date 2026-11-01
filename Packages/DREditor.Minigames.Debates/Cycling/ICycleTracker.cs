//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public interface ICycleTracker
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="immediate"></param>
        /// <returns></returns>
        float MoveToFirstPosition(Transform transform, bool immediate = false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="immediate"></param>
        void MoveAll(bool forward, bool immediate = false);

        /// <summary>
        ///
        /// </summary>
        void ClearTrack();
    }
}