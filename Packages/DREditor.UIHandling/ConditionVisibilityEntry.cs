//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ConditionVisibilityEntry : VisibilityEntryBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition condition;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool invert = false;


        /// <inheritdoc/>
        protected override void EvaluateInState(bool inState)
        {
            if (condition != null && condition.Evaluate())
                setVisibility(invert ? inState : !inState);
        }
    }
}