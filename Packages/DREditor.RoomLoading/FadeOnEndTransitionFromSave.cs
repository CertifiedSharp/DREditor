//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using DREditor.UIHandling;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISceneEventListener) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class FadeOnEndTransitionFromSave : MonoBehaviour, ISceneEventListener, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadeOutComplete = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadingOutOfLoading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent screenIsDark = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedRoomTransitions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float loadingFileFadeOutTime = 0.3f;


        /// <summary>
        ///
        /// </summary>
        protected bool subscribed = false;


        protected virtual void OnEnable() => isLoadingFile.AddListener(OnLoadingFileStateChanged);

        protected virtual void OnDisable() => isLoadingFile.RemoveListener(OnLoadingFileStateChanged);

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadingFile"></param>
        protected virtual void OnLoadingFileStateChanged(bool loadingFile)
        {
            if (loadingFile)
                SubscribeToEvents();
            else
                UnsubscribeToEvents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
            if (!subscribed)
            {
                onFadeOutComplete.RegisterListener(this);

                onFinishedRoomTransitions.Response.AddListener(FadeOutOnTransitionsFinished);

                subscribed = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToEvents()
        {
            if (subscribed)
            {
                onFadeOutComplete.UnregisterListener(this);

                onFinishedRoomTransitions.Response.RemoveListener(FadeOutOnTransitionsFinished);

                StopAllCoroutines();

                subscribed = false;
            }
        }

        /// <inheritdoc/>
        public virtual void OnEventRaised() => OnEventInvoke(InvokeFadingOutOfLoading);

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        protected virtual void OnEventInvoke(Action action)
        {
            if (isLoadingFile.Value)
            {
                UnsubscribeToEvents();

                action?.Invoke();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeFadingOutOfLoading()
        {
            if (onFadingOutOfLoading != null)
                onFadingOutOfLoading.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeOutOnTransitionsFinished() => OnEventInvoke(WaitForLoadingToFinish);

        /// <summary>
        ///
        /// </summary>
        protected virtual void WaitForLoadingToFinish() => StartCoroutine(WaitForLoadingToFinishRoutine());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForLoadingToFinishRoutine()
        {
            yield return WaitForLoadingOrFadeToFinish();

            yield return WaitForAnimationsToComplete();

            InvokeFadingOutOfLoading();

            FadeOutOfLoading();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForLoadingOrFadeToFinish()
        {
            while (isLoadingFile.Value && screenIsDark.Value)
                yield return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForAnimationsToComplete()
        {
            var waitableAnimations = FindObjectsByType<MonoBehaviour>().OfType<IWaitAnimationCompleteOnLoad>();

            foreach (var animation in waitableAnimations)
                yield return animation.WaitForAnimationToComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeOutOfLoading()
        {
            if (!ServiceLocator.HasService<GlobalFade>(out var globalFade) ||
                !ServiceLocator.HasService<UIHandlerBase>(out var uiHandler))
                return;

            bool fadeOutOptions =
                !ServiceLocator.HasService<RoomChangerBase>(out var roomChanger) ||
                roomChanger.GetOptions() == null ||
                roomChanger.GetOptions().fadeOutOnLoad;

            if (screenIsDark.Value && !uiHandler.MovingToTitleScreen && fadeOutOptions)
                globalFade.FadeOut(loadingFileFadeOutTime);
            else
                UnityEngine.Debug.Log("REPORT: Didn't Fade out of loading.");
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => UnsubscribeToEvents();
    }
}