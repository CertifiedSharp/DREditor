//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    public abstract class HealthStatBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        ///
        /// </summary>
        /// <param name="damage"></param>
        public abstract void TakeDamage(float damage = 0);

        /// <summary>
        ///
        /// </summary>
        public abstract void ResetHealth();
    }
}