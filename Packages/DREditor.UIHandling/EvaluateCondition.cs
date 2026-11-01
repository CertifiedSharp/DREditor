//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class EvaluateCondition : MonoBehaviour
    {
        [Header("")]
        [SerializeField] protected UnityEvent onConditionPassed;

        [Header("")]
        [SerializeField] protected UnityEvent onConditionFailed;

        [Header("")]
        [SerializeField] protected UnityEvent<bool> onConditionEvaluated;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public void InvokeEvaluate() => Evaluate();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool Evaluate()
        {
            bool result = OnEvaluateCondition();

            InvokeResultEvents(result);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool OnEvaluateCondition();

        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        protected virtual void InvokeResultEvents(bool result)
        {
            OnConditionEvaluated(result);

            if (result)
                OnConditionPassed();
            else
                OnConditionFailed();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        protected virtual void OnConditionEvaluated(bool result) => onConditionEvaluated?.Invoke(result);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnConditionPassed() => onConditionPassed?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnConditionFailed() => onConditionFailed?.Invoke();
    }
}