//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public class IntCurrencyHandler : CurrencyHandler<int>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currency = null;

        /// <inheritdoc/>
        public override void AddCurrency(int amount)
        {
            if (currency.Value <= int.MaxValue - amount)
                currency.Value += amount;
        }

        /// <inheritdoc/>
        public override int GetCurrency() => currency.Value;

        /// <inheritdoc/>
        public override bool CanPay(int amount) => currency.Value >= amount && currency.Value > 0;

        /// <inheritdoc/>
        public override void RemoveCurrency(int amount) => currency.Value -= amount;

        /// <inheritdoc/>
        public override void ClearData() => currency.SetValue(0);

        /// <inheritdoc/>
        public override bool IsBroke() => currency.Value == 0;
    }
}