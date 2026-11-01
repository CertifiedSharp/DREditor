//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Processables;
using DREditor.Services;
using DREditor.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class FTEPurchaseCompleteHandler : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent onPurchaseCompleted = null;

        [Header("")]
        public UnityEvent onSurfacePurchaseCompleted;


        /// <inheritdoc/>
        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            OnPurchased();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPurchased() =>
            ServiceLocator.GetService<IFTESurfaceManager>()
            .GetSelectedSurface()
            .InvokeThisAs<ISubmittable>(InvokeSubmitableComponent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="submitable"></param>
        protected virtual void InvokeSubmitableComponent(ISubmittable submitable) =>
            submitable.Submit(onComplete: OnPurchaseCompleted);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPurchaseCompleted()
        {
            onSurfacePurchaseCompleted?.Invoke();

            if (onPurchaseCompleted != null)
                onPurchaseCompleted.Raise();
        }
    }
}