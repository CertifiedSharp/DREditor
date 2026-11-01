//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SubmittableFTESurfaceComponent) + ", " + nameof(FTESurfaceComponent) + ", " + nameof(IComponentsCache) +
        "\n\n")]
    public class SubmittableFTESurfaceCollection : SubmittableFTESurfaceComponent, IComponentsCache
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool logAmountCompleted = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<FTESurfaceComponent> components = new();


        /// <summary>
        ///
        /// </summary>
        protected int totalCompleted = 0;

        /// <summary>
        ///
        /// </summary>
        protected Action onComplete;


        public virtual MonoBehaviour[] CachedComponents => components.ToArray();


        /// <inheritdoc/>
        protected override void InitializeCallback(Action onComplete) => this.onComplete = onComplete;

        /// <inheritdoc/>
        protected override void OnSubmit() => 
            this.InvokeComponents<ISubmittable>((submittable) => submittable.Submit(OnComponentCompleted));

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnComponentCompleted()
        {
            UpdateComponentsCompleted();

            CheckCompletedComponents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateComponentsCompleted() => totalCompleted++;

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckCompletedComponents()
        {
            if (AllComponentsCompleted())
                InvokeCompleted();
            else
                WhenNotAllComponentsCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AllComponentsCompleted() =>
            totalCompleted == components
            .OfType<ISubmittable>()
            .ToArray()
            .Length;

        /// <summary>
        ///
        /// </summary>
        protected override void InvokeCompleted()
        {
            totalCompleted = 0;

            onComplete?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void WhenNotAllComponentsCompleted()
        {
            if (logAmountCompleted)
                UnityEngine.Debug.Log($"REPORT: {gameObject.name}: " +
                    $"{totalCompleted} out of {components.OfType<ISubmittable>().ToArray().Length} completed.");
        }
    }
}