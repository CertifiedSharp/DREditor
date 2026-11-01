//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine.Events;
using UnityEngine;
using DREditor.EventObjects;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(ServiceHolder<TransitionFinishedSubscriber>) + " for type: " + nameof(TransitionFinishedSubscriber) +
        "\n\n")]
    public class TransitionFinishedSubscriber : ServiceHolder<TransitionFinishedSubscriber>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedRoomTransitionsListener = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void SubscribeToRoomTransitionsFinished(UnityAction action)
            => onFinishedRoomTransitionsListener.Response.AddListener(action);

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void UnsubscribeToRoomTransitionsFinished(UnityAction action)
            => onFinishedRoomTransitionsListener.Response.RemoveListener(action);
    }
}