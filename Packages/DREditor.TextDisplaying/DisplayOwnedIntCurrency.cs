//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DisplayFormattedString) +
        "\n\n")]
    public class DisplayOwnedIntCurrency : DisplayFormattedString
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currency = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent paidAmount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool neverNegative = true;


        /// <summary>
        ///
        /// </summary>
        protected readonly int Zero = 0;


        /// <inheritdoc/>
        protected override object GetObjectToBeFormatted() => ResultIsInBounds() ? RemovedFromCurrency() : Zero;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ResultIsInBounds() => !neverNegative || paidAmount.Value <= currency.Value;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float RemovedFromCurrency() => currency.Value - paidAmount.Value;

    }
}