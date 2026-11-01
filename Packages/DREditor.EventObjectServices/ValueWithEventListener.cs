//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public abstract class ValueWithEventListener<T, TX, TY> : MonoBehaviour
        where T : ValueWithEvent<TX, TY>
        where TY : UnityEvent<TX>, new()
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected T valueWithEvent = null;

        [Space]

        [Header("")]
        [SerializeField] protected TY OnValueChangedEvent;


        /// <summary>
        ///
        /// </summary>
        protected TX UpdatedValue { get; private set; }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => valueWithEvent?.AddListener(OnValueChanged);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => valueWithEvent?.RemoveListener(OnValueChanged);


        /// <summary>
        ///
        /// </summary>
        /// <param name="updatedValue"></param>
        protected virtual void OnValueChanged(TX updatedValue)
        {
            UpdatedValue = updatedValue;

            OnValueChangedEvent?.Invoke(updatedValue);
        }
    }
}