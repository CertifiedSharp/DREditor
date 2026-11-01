//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
    public class PostProcessingColorEventHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Graphic topGraphic = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Graphic bottomGraphic = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<string> setHSVEventStrings = new() { "SetHSV" };

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> destroyPostProcessingEventStrings = new() { "DestroyPostProcess" };


        /// <summary>
        ///
        /// </summary>
        protected Color originalHSVColor;

        /// <summary>
        ///
        /// </summary>
        protected bool initializedColor = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            SetOriginalProtagHSV();

            SubscribeToEvents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetOriginalProtagHSV()
        {
            if (!initializedColor)
            {
                originalHSVColor = topGraphic.color;

                initializedColor = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void OnPostProcessEffectChange(object o = null) => SetProtagSpriteHSV((Color)o);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void OnPostProcessDestroyed(object o = null) => SetProtagSpriteHSV(originalHSVColor);

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        protected virtual void SetProtagSpriteHSV(Color color)
        {
            SetOriginalProtagHSV();

            ApplyColorToGraphics(color);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        protected virtual void ApplyColorToGraphics(Color color)
        {
            if (topGraphic != null)
                topGraphic.ApplyColorNoAlpha(color);

            if (bottomGraphic != null)
                bottomGraphic.ApplyColorNoAlpha(color);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            SetProtagSpriteHSV(originalHSVColor);

            UnsubscribeToEvents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
            StartListeningToEvents(setHSVEventStrings, OnPostProcessEffectChange);

            StartListeningToEvents(destroyPostProcessingEventStrings, OnPostProcessDestroyed);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToEvents()
        {
            StopListeningToEvents(setHSVEventStrings, OnPostProcessEffectChange);

            StopListeningToEvents(destroyPostProcessingEventStrings, OnPostProcessDestroyed);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventStrings"></param>
        /// <param name="listener"></param>
        protected virtual void StartListeningToEvents(List<string> eventStrings, UnityAction<object> listener)
        {
            foreach (string eventString in eventStrings)
                DialogueEventSystem.StartListening(eventString, listener);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventStrings"></param>
        /// <param name="listener"></param>
        protected virtual void StopListeningToEvents(List<string> eventStrings, UnityAction<object> listener)
        {
            foreach (string eventString in eventStrings)
                DialogueEventSystem.StopListening(eventString, listener);
        }
    }
}