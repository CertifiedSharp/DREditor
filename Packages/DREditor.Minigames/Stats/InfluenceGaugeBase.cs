//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    public abstract class InfluenceGaugeBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => ServiceLocator.Register<InfluenceGaugeBase>(this);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => ServiceLocator.Unregister<InfluenceGaugeBase>(this);


        /// <summary>
        ///
        /// </summary>
        public abstract void DrainStaminaVisual();

        /// <summary>
        ///
        /// </summary>
        public abstract bool StaminaVisualWrong();

        /// <summary>
        ///
        /// </summary>
        public abstract void RegenStaminaVisual();

        /// <summary>
        ///
        /// </summary>
        public abstract void ResetStamina();
    }
}