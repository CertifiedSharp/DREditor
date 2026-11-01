//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ConditionOnSceneEvent : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected EvaluateCondition condition = null;


        /// <inheritdoc/>
        public override void OnEventRaised()
        {
            base.OnEventRaised();

            OnCheckCondition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCheckCondition()
        {
            if (condition == null || condition.Evaluate())
                OnPassedCondition();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnPassedCondition();
    }
}