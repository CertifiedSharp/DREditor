//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PurchaseInputHandlerBase : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onPurchase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onCantPurchase = null;


        [Header("")]
        public UnityEvent OnPurchaseMade;

        [Header("")]
        public UnityEvent OnCantPurchase;


        /// <inheritdoc/>
        public override void ActivateControls() => SubscribePurchase(Purchase);

        /// <inheritdoc/>
        public override void DeactivateControls() => UnsubscribePurchase(Purchase);

        /// <summary>
        ///
        /// </summary>
        /// <param name="purchase"></param>
        protected abstract void SubscribePurchase(Action<CallbackContext> purchase);

        /// <summary>
        ///
        /// </summary>
        /// <param name="purchase"></param>
        protected abstract void UnsubscribePurchase(Action<CallbackContext> purchase);


        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void Purchase(CallbackContext ctx)
        {
            AttemptPurchase();

            DisplayAvailableCurrency();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptPurchase()
        {
            if (PaymentCanBeMade())
                OnMakePurchase();
            else
                OnCantMakePurchase();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool PaymentCanBeMade();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnMakePurchase()
        {
            DisableInput();

            MakePayment();

            InvokeOnPurchaseMade();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void MakePayment();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeOnPurchaseMade()
        {
            OnPurchaseMade?.Invoke();

            if (onPurchase != null)
                onPurchase.Raise();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCantMakePurchase() => InvokeOnCantPurchase();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeOnCantPurchase()
        {
            OnCantPurchase?.Invoke();

            if (onCantPurchase != null)
                onCantPurchase.Raise();
        }


        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayAvailableCurrency();
    }
}