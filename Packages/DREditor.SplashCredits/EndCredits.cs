//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DREditor.SplashCredits
{
    /// <summary>
    ///
    /// </summary>
    public class EndCredits : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected InputActionReference pause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onLeavingCredits = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string menuScene = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI skipText = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected float startFadeOutDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float skipTextFadeToValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float skipTextFadeToDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float skipTextFadeOutValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float skipTextFadeOutDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float leaveFadeToDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected bool canSkip = false;

        /// <summary>
        ///
        /// </summary>
        protected bool skipping = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => StartCredits();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartCredits()
        {
            AddSkipInput();

            StartCoroutine(FadeOutService());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => RemoveSkipInput();

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddSkipInput() => pause.action.started += OnSkipInput;

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveSkipInput() => pause.action.started -= OnSkipInput;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator FadeOutService()
        {
            while (ServiceLocator.GetService<GlobalFade>().isTransitioning)
                yield return null;

            if (ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeOut(startFadeOutDuration);

            yield break;
        }

        
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <remarks></remarks>
        protected virtual void OnSkipInput(InputAction.CallbackContext context)
        {
            UnityEngine.Debug.Log($"{ToString()}: Called Skip");

            if (skipping)
                return;

            if (canSkip)
                LeaveCredits();
            else
                StartCoroutine(ShowTextToSkipCredits());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ShowTextToSkipCredits()
        {
            UnityEngine.Debug.Log($"{ToString()}: Revealing Skip Text.");

            canSkip = true;
#if DOTWEEN
            skipText.DOFade(skipTextFadeToValue, skipTextFadeToDuration);

            yield return new WaitForSecondsRealtime(skipTextFadeToDuration);

            skipText.DOFade(skipTextFadeOutValue, skipTextFadeOutDuration);
#endif
            yield return new WaitForSecondsRealtime(skipTextFadeOutDuration);

            canSkip = false;
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void LeaveCredits() => StartCoroutine(LeaveCreditsRoutine());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LeaveCreditsRoutine()
        {
            skipping = true;

            onLeavingCredits.Raise();

            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(leaveFadeToDuration);

            SceneManager.LoadSceneAsync(menuScene);
        }
    }
}