//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HealthDataHandler<T> : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="damage"></param>
        public abstract void TakeDamage(T damage);

        /// <summary>
        ///
        /// </summary>
        public abstract void ResetHealth();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract T GetHealth();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract T GetMaxHealth();
    }
}