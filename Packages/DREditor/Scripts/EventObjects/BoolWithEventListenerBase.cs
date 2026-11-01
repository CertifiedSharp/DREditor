//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BoolWithEventListenerBase<T> : MonoBehaviour
    {
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected T boolWithEvent;


        [Header("")]

        [Header("")]
        [SerializeField] protected UnityEvent onTrueEvent = null;

        [Header("")]
        [SerializeField] protected UnityEvent onFalseEvent = null;


        protected virtual void OnEnable() => Subscribe();

        protected virtual void OnDisable() => Unsubscribe();

        /// <summary>
        ///
        /// </summary>
        protected abstract void Subscribe();

        /// <summary>
        ///
        /// </summary>
        protected abstract void Unsubscribe();

        /// <summary>
        ///
        /// </summary>
        /// <param name="changedValue"></param>
        protected virtual void OnEventChanged(bool changedValue)
        {
            if (changedValue)
                OnEventTrue();
            else
                OnEventFalse();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEventTrue() => onTrueEvent?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEventFalse() => onFalseEvent?.Invoke();
    }
}