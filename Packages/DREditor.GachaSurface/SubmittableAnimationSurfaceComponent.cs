//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FreeTimeSurfacing;
using DREditor.Processables;
using DREditor.UIHandling;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.GachaSurface
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SubmittableAnimationSurfaceComponent : SubmittableFTESurfaceComponent, IStartUpable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiVisibilityComponent = null;


        /// <summary>
        ///
        /// </summary>
        protected Action onComplete;


        /// <inheritdoc/>
        public virtual void StartUp() => uiVisibilityComponent.Hide();


        /// <inheritdoc/>
        protected override void InitializeCallback(Action onComplete) => this.onComplete = onComplete;

        /// <inheritdoc/>
        protected override void OnSubmit() => StartCoroutine(DisplayThenCompleteRolledAnimation());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplayThenCompleteRolledAnimation()
        {
            yield return HideThenShowRoutine();

            InvokeCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator HideThenShowRoutine()
        {
            uiVisibilityComponent.Hide();

            yield return uiVisibilityComponent.ShowTillComplete();
        }

        /// <inheritdoc/>
        protected override void InvokeCompleted() => onComplete?.Invoke();
    }
}