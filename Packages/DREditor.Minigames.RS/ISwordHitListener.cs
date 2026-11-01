//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    public interface ISwordHitListener
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="hitObject"></param>
        /// <param name="hitPoint"></param>
        void OnSwordHit(GameObject hitObject, Vector3 hitPoint);
    }
}