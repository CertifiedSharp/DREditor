//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Linq;
using UnityEngine;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public abstract class ActuatorBehaviourBase : MonoBehaviour, IActuatorBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (ServiceLocator.HasService<TransitionFinishedSubscriber>())
                ServiceLocator.GetService<TransitionFinishedSubscriber>().SubscribeToRoomTransitionsFinished(InitializeActuator);
            else
                UnityEngine.Debug.LogWarning(gameObject.name + " Actuator: ALERT: The service for when the room transition finishes" +
                    " could not be found, therefore, this actuator will not initialize.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (ServiceLocator.HasService<TransitionFinishedSubscriber>())
                ServiceLocator.GetService<TransitionFinishedSubscriber>().UnsubscribeToRoomTransitionsFinished(InitializeActuator);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void InitializeActuator();


        /// <inheritdoc/>
        public abstract object Save();

        /// <inheritdoc/>
        public abstract void Load(object ob);

        /// <inheritdoc/>
        public virtual void Call()
        {
            var subsequents = GetSubsequents();

            foreach (ISubsequentBehaviour subsequent in subsequents)
                subsequent.Call();
        }

        /// <inheritdoc/>
        public virtual ISubsequentBehaviour[] GetSubsequents() =>
            GetComponents<MonoBehaviour>().OfType<ISubsequentBehaviour>().ToArray();
    }
}