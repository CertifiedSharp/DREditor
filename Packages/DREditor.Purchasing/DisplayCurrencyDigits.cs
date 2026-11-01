//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
using DREditor.Saving.Handlers;
using DREditor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(DisplayDigits) + 
        "\n\n")]
    public class DisplayCurrencyDigits : DisplayDigits
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<IntWithEventListener> digits = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CurrencyHandler<int> currencyHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent paidAmount = null;


        /// <summary>
        ///
        /// </summary>
        protected IDigitPlace selectedDigit;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            paidAmount.SetValue(Zero);
        }


        /// <inheritdoc/>
        protected override void InitializeSelectedDigit() =>
            selectedDigit = digits.GetSelectedObjectOfType<IDigitPlace>();


        /// <inheritdoc/>
        protected override void ProcessInputAmount()
        {
            if (DigitsShouldBeUpdated())
                UpdateDigitsFrom(CalculateDigitAmount());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DigitsShouldBeUpdated() => HandlersValid() && !DecreasingOnZero() && !IncreasingWhenDigitsMaxed();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool HandlersValid() => selectedDigit != null && currencyHandler != null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DecreasingOnZero() => directionalRate <= Zero && paidAmount.Value == Zero;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool IncreasingWhenDigitsMaxed() =>
            directionalRate == inputDirectionalRate && ForwardDigitsMaxed(selectedDigit);

        /// <summary>
        ///
        /// </summary>
        /// <param name="starting"></param>
        /// <returns></returns>
        protected virtual bool ForwardDigitsMaxed(IDigitPlace starting) =>
            digits.OfType<IDigitPlace>()
            .SkipWhile(digit => digit != starting)
            .All(digit => digit.DisplayingMaxDigit());


        /// <summary>
        ///
        /// </summary>
        /// <param name="amount"></param>
        protected virtual void UpdateDigitsFrom(int amount)
        {
            if (currencyHandler.GetCurrency() + amount < Zero || paidAmount.Value + amount < Zero)
                paidAmount.SetValue(Zero);
            else if (paidAmount.Value + amount <= currencyHandler.GetCurrency())
                paidAmount.SetValue(paidAmount.Value + amount);
            else
                paidAmount.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int CalculateDigitAmount() => selectedDigit.GetDigitPlace() * directionalRate;
    }
}