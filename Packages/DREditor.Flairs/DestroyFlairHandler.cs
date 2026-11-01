//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DestroyFlairHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FlairDestroyer destroyer = null;


        [Header("")]
        public UnityEvent onOtherHandlerInvoked = null;


        /// <summary>
        ///
        /// </summary>
        protected DestroyFlairHandler[] otherHandlers;


        protected virtual void Awake() =>
            otherHandlers = GetComponents<DestroyFlairHandler>()
            .Where(handler => handler != this)
            .ToArray();

        protected virtual void Start() =>
            UpdateFlairSubscription((handler) => handler.onOtherHandlerInvoked.AddListener(OnOtherHandlerDestroying));

        protected virtual void OnDestroy() =>
            UpdateFlairSubscription((handler) => handler.onOtherHandlerInvoked.RemoveListener(OnOtherHandlerDestroying));

        /// <summary>
        ///
        /// </summary>
        /// <param name="update"></param>
        protected virtual void UpdateFlairSubscription(Action<DestroyFlairHandler> update)
        {
            if (otherHandlers == null)
                return;

            foreach (DestroyFlairHandler handler in otherHandlers)
                if (handler != null)
                    update(handler);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnOtherHandlerDestroying();

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptToDestroyFlair()
        {
            if (FlairCanBeDestroyed())
                InvokeFlairDestroyer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool FlairCanBeDestroyed() => gameObject != null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeFlairDestroyer()
        {
            onOtherHandlerInvoked?.Invoke();

            if (destroyer != null)
                destroyer.DestroyFlair();
            else
                UnityEngine.Debug.LogWarning("ALERT: This flair cannot be destroyed! " +
                    "\nANALYSIS: A destroyer has not been assigned!");
        }
    }
}