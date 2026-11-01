//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using System;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SubmittableFTESurfaceComponent : FTESurfaceComponent, ISubmittable
    {
        /// <inheritdoc/>
        public virtual void Submit(Action onComplete)
        {
            InitializeCallback(onComplete);

            OnSubmit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="onComplete"></param>
        protected abstract void InitializeCallback(Action onComplete);

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnSubmit();

        /// <summary>
        ///
        /// </summary>
        protected abstract void InvokeCompleted();
    }
}