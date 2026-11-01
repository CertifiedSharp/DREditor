//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving.Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HealthStatBase) +
        "\n\n")]
    public class HealthStat : HealthStatBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected HealthDataHandler<int> healthDataHandler = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("colorHandlers")]
        [SerializeField] protected List<HealthStatBase> healthModifiers = new();


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent OnDamageTaken = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnHealthRestored = null;


        /// <inheritdoc/>
        public override void ResetHealth()
        {
            OnHealthRestored?.Invoke();

            ForeachModifier(handler => handler.ResetHealth());

            healthDataHandler.ResetHealth();
        }

        /// <inheritdoc/>
        public override void Refresh() => ForeachModifier(handler => handler.Refresh());

        /// <inheritdoc/>
        public override void TakeDamage(float damage)
        {
            OnDamageTaken?.Invoke();

            ForeachModifier(handler => handler.TakeDamage(damage));

            healthDataHandler.TakeDamage((int)damage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        protected virtual void ForeachModifier(Action<HealthStatBase> action)
        {
            foreach (var modifier in healthModifiers)
                if (modifier != null && modifier != this)
                    action(modifier);
        }
    }
}