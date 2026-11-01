//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class FadeOutOnRoomLoad : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedRoomTransitions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string fadeOutEventString = "FadeOutOnLoadSubscribe";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => StartListeningToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartListeningToEvents()
            => DialogueEventSystem.StartListening(fadeOutEventString, SubscribeFadeToOnLoad);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => StopListeningToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopListeningToEvents()
            => DialogueEventSystem.StopListening(fadeOutEventString, SubscribeFadeToOnLoad);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void SubscribeFadeToOnLoad(object o = null) 
            => onFinishedRoomTransitions.Response.AddListener(FadeOnLoad);

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeOnLoad()
        {
            onFinishedRoomTransitions.Response.RemoveListener(FadeOnLoad);

            if (ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutDuration);
        }
    }
}