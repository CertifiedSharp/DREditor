//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving.Handlers;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PurchaseInputHandlerBase) + ", " + nameof(ControlledComponent) +
        "\n\n")]
    public class IntPurchaseInputHandler : PurchaseInputHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent paidAmount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CurrencyHandler<int> currencyHandler = null;


        /// <inheritdoc/>
        protected override void SubscribePurchase(Action<CallbackContext> purchase) =>
            submit.action.started += purchase;

        /// <inheritdoc/>
        protected override void UnsubscribePurchase(Action<CallbackContext> purchase) => 
            submit.action.started -= purchase;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            if (paidAmount == null)
                UnityEngine.Debug.LogError($"ALERT: Field: {nameof(paidAmount)} MUST be assigned to use this functionality.");

            if (currencyHandler == null)
                UnityEngine.Debug.LogError($"ALERT: Field: {nameof(currencyHandler)} MUST be assigned to use this functionality.");
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            DisplayAvailableCurrency();
        }

        /// <inheritdoc/>
        protected override void DisplayAvailableCurrency() => paidAmount.Invoke();

        /// <inheritdoc/>
        protected override bool PaymentCanBeMade() => paidAmount.Value != 0 &&
            currencyHandler != null && currencyHandler.CanPay(paidAmount.Value) && !currencyHandler.IsBroke();

        /// <inheritdoc/>
        protected override void MakePayment() => currencyHandler.RemoveCurrency(paidAmount.Value);
    }
}