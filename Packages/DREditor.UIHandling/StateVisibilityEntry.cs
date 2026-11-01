//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class StateVisibilityEntry : VisibilityEntryBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool invert = false;


        /// <inheritdoc/>
        protected override void EvaluateInState(bool inState)
        {
            if (inState || (invert && !inState))
                setVisibility(!invert ? !inState : inState);
        }
    }
}