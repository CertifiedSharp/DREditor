//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.SplashCredits
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<ILoadScreenManager>) + " for type: " + nameof(ILoadScreenManager) +
        "\n\n")]
    public class LoadScreenManager : ServiceHolder<ILoadScreenManager>, ILoadScreenManager
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
        [SerializeField] protected Canvas loadCanvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image loadingBar = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string toSceneOnLoad;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int unloadSceneIndex = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool clickOnLoadComplete = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Graphic blackFadeOverlay;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutOverlayValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutOverlayDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToSceneOverlayValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToSceneOverlayDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected AsyncOperation async = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => loadCanvas.enabled = false;

        
        /// <inheritdoc/>
        public virtual void StartLoadingScreen() => StartCoroutine(LoadRoutine());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LoadRoutine()
        {
            loadCanvas.enabled = true;

            FadeOutOverlay();

            yield return new WaitForSeconds(fadeOutOverlayDuration);

            LoadSceneAsync();

            yield return LoadProgressBar();

            FadeOnProgressComplete();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeOutOverlay()
        {
#if DOTWEEN
            if (blackFadeOverlay != null)
                blackFadeOverlay.DOFade(fadeOutOverlayValue, fadeOutOverlayDuration);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadSceneAsync()
        {
            async = SceneManager.LoadSceneAsync(toSceneOnLoad, LoadSceneMode.Additive);

            async.allowSceneActivation = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LoadProgressBar()
        {
            float progressValue;

            while (async.progress < 0.9f)
            {
                progressValue = Mathf.Clamp01(async.progress / 0.9f);

                loadingBar.fillAmount = progressValue;

                yield return null;
            }

            progressValue = Mathf.Clamp01(async.progress / 0.9f);

            loadingBar.fillAmount = progressValue;

            UnityEngine.Debug.Log($"{ToString()}: Loading Progress Bar Complete!");

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeOnProgressComplete()
        {
            if (clickOnLoadComplete)
                submit.action.started += ConfirmLoad;
            else
                FadeToScene();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ConfirmLoad(CallbackContext context)
        {
            submit.action.started -= ConfirmLoad;

            FadeToScene();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FadeToScene()
        {
#if DOTWEEN
            blackFadeOverlay.DOFade(fadeToSceneOverlayValue, fadeToSceneOverlayDuration)
                .OnComplete(ActivateScene);
#endif
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateScene()
        {
            SceneManager.sceneLoaded += UnloadSceneOnLoad;

            async.allowSceneActivation = true;
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected virtual void UnloadSceneOnLoad(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= UnloadSceneOnLoad;

            if (loadCanvas != null)
                loadCanvas.enabled = false;

            SceneManager.UnloadSceneAsync(unloadSceneIndex);
        }
    }
}