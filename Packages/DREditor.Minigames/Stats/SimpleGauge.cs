//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [MovedFrom(true, "DREditor.Minigames.Debates.Absorbing", "DREditor.Minigames.Debates", "AbsorbGauge")]
    public class SimpleGauge : IGauge
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float currentCharge = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float filledGaugeAmount = 1;


        public virtual float Current { get => currentCharge; private set => currentCharge = value; }

        public virtual bool IsCharged => Current >= filledGaugeAmount;

        public virtual bool IsEmpty => Current <= 0;


        /// <inheritdoc/>
        public virtual void Tick(float deltaTime)
        {
            Current += deltaTime;

            if (Current > filledGaugeAmount)
                Current = filledGaugeAmount;
        }

        /// <inheritdoc/>
        public virtual void Reset() => Current = 0;
    }
}