//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public class IntHealthDataHandler : HealthDataHandler<int>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent health = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int MaxHealth = 10;

        /// <inheritdoc/>
        public override void TakeDamage(int damage)
        {
            if (health.Value - damage < 0)
                health.SetValue(0);
            else
                health.SetValue(health.Value -= damage);
        }

        /// <inheritdoc/>
        public override void ResetHealth() => health.SetValue(MaxHealth);

        /// <inheritdoc/>
        public override int GetHealth() => health.Value;

        /// <inheritdoc/>
        public override int GetMaxHealth() => MaxHealth;
    }
}