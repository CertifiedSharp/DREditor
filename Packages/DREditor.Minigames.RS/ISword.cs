//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    public interface ISword
    {
        /// <summary>
        ///
        /// </summary>
        public bool RecentlyLandedHit { get; }

        /// <summary>
        ///
        /// </summary>
        public UnityEvent<GameObject, Vector3> OnHitDetected { get; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        Vector3 AngleSword(Vector3 direction);

        /// <summary>
        ///
        /// </summary>
        void StartDetectingCuts();

        /// <summary>
        ///
        /// </summary>
        void StopDetectingCuts();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool IsDetectingCuts();
    }
}