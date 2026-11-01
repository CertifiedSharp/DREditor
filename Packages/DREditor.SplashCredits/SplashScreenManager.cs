//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.SplashCredits
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class SplashScreenManager : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool displayOnStart = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage mainSplashImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Graphic blackFadeOverlay = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Canvas splashCanvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SplashScreen> splashScreens = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float overlayChangeSplashToValue = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float overlayChangeSplashOutValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float overlayChangeDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float overlayStartValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float overlayStartDuration = 0f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected VideoPlayer videoPlayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RenderTexture videoPlayerRenderTexture = null;

        [Space]

        [Header("")]
        [SerializeField] protected UnityEvent OnSplashScreensComplete;


        /// <summary>
        ///
        /// </summary>
        protected SplashScreen currentScreen = null;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine currentScreenRoutine = null;

        /// <summary>
        ///
        /// </summary>
        protected bool screenComplete = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => Initialize();

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
#if DOTWEEN
            if (blackFadeOverlay != null)
                blackFadeOverlay.DOFade(overlayStartValue, overlayStartDuration);
#endif
            if (displayOnStart)
                StartDisplayingSplashScreens();
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void StartDisplayingSplashScreens() => StartCoroutine(DisplayAllSplashScreensRoutine());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplayAllSplashScreensRoutine()
        {
            foreach (SplashScreen splashScreen in splashScreens)
                yield return DisplayInteractableSplashScreen(splashScreen);

            ResolveSplashVisuals();

            OnSplashScreensComplete?.Invoke();

            ServiceLocator.GetService<ILoadScreenManager>()?.StartLoadingScreen();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="splashScreen"></param>
        protected virtual IEnumerator DisplayInteractableSplashScreen(SplashScreen splashScreen)
        {
            currentScreenRoutine = StartCoroutine(DisplaySplash(splashScreen));

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Waiting for the Screen to not be complete to add Skip Function");

            yield return new WaitUntil(() => !screenComplete);

            submit.action.started += AttemptToSkipScreen;

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Waiting for Splash Screen to Complete");

            yield return new WaitUntil(() => screenComplete);

            submit.action.started -= AttemptToSkipScreen;

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Splash Screen Complete");

            StopCoroutine(currentScreenRoutine);

            yield return FadeOverlayTo(overlayChangeSplashToValue, overlayChangeDuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="splashScreen"></param>
        protected virtual IEnumerator DisplaySplash(SplashScreen splashScreen)
        {
            currentScreen = splashScreen;

            yield return DisplaySplashVisual();

            yield return FadeOverlayTo(overlayChangeSplashOutValue, overlayChangeDuration);

            screenComplete = false;

            yield return StartCoroutine(WaitForScreenToComplete()); // Links so stops with this routine.

            screenComplete = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplaySplashVisual()
        {
            if (currentScreen.clip != null)
                yield return DisplaySplashVideo();
            else
                SetScreenTexture();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetScreenTexture()
        {
            if (currentScreen.Screen != null)
                mainSplashImage.texture = currentScreen.Screen;
            else
                UnityEngine.Debug.LogError($"{ToString()}: The current splash screen: " + currentScreen.name + " doesn't have a texture" +
                    " when you are trying to set it! Ensure that the screen texture is filled if it's not a video or another medium.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplaySplashVideo()
        {
            mainSplashImage.texture = videoPlayerRenderTexture;

            videoPlayer.clip = currentScreen.clip;

            videoPlayer.Prepare();

            while (!videoPlayer.isPrepared)
                yield return null;

            videoPlayer.Play();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="overlayToValue"></param>
        /// <param name="overlayDuration"></param>
        protected virtual IEnumerator FadeOverlayTo(float overlayToValue, float overlayDuration)
        {
#if DOTWEEN
            blackFadeOverlay.DOFade(overlayToValue, overlayDuration);
#endif
            yield return new WaitForSeconds(overlayDuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual IEnumerator WaitForScreenToComplete()
        {
            if (currentScreen.clip != null)
                yield return new WaitForSeconds((float)currentScreen.clip.length);
            else
                yield return new WaitForSeconds(currentScreen.screenTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void AttemptToSkipScreen(CallbackContext ctx)
        {
            if (currentScreen != null && !screenComplete && currentScreen.skippable)
                screenComplete = true;

            LogSkippingScreen();
        }

        /// <summary>
        ///
        /// </summary>
        private void LogSkippingScreen()
        {
            if (currentScreen != null)
                if (!screenComplete && currentScreen.skippable)
                    UnityEngine.Debug.Log($"REPORT: {ToString()}: Skipping Splash Screen");
                else
                    UnityEngine.Debug.Log($"REPORT: " +
                        $"{ToString()}: Splash Screen was either completed or not skippable when attempting to skip.");
            else
                UnityEngine.Debug.Log($"REPORT: {ToString()}: Current Splash Screen is null");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveSplashVisuals()
        {
            screenComplete = false;

            if (splashCanvas != null)
                splashCanvas.enabled = false;
        }
    }
}