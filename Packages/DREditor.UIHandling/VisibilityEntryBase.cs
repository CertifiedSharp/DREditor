//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class VisibilityEntryBase : IModeVisibilityEntry
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent eventChecked;


        /// <summary>
        ///
        /// </summary>
        protected Action<bool> setVisibility;


        /// <inheritdoc/>
        public virtual void Initialize(Action<bool> setVisibility)
        {
            this.setVisibility = setVisibility;

            eventChecked.AddListener(EvaluateInState);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inState"></param>
        protected abstract void EvaluateInState(bool inState);

        /// <inheritdoc/>
        public virtual void Resolve() => eventChecked.RemoveListener(EvaluateInState);
    }
}