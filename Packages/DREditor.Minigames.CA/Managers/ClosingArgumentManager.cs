// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Minigames.Stats;
using DREditor.Services;
using DREditor.UIHandling;
using DREditor.UIHandling.Reticles;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.CA.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MinigameManagerBase) + ", " + nameof(ClosingArgumentManagerBase) +
        "\n\n")]
    public class ClosingArgumentManager : ClosingArgumentManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent caResult = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected HoverReticle reticle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float minigameEndFadeDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int damageTakenOnWrong = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent introOutroUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent resultUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent answerUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent cameraUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float menuFadeWaitTime = 0.5f;


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent OnCorrect = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnIncorrect = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnMinigameEnding = null;


        /// <summary>
        ///
        /// </summary>
        protected int totalAnswered = 0;


        /// <summary>
        ///
        /// </summary>
        protected IActiveStockPanel activeStockPanel;

        /// <summary>
        ///
        /// </summary>
        protected IClosingArgumentListener[] argumentListeners;

        /// <summary>
        ///
        /// </summary>
        protected IClosingArgumentResetable[] argumentReseters;


        /// <inheritdoc/>
        protected override void OnMinigameEnable()
        {
            base.OnMinigameEnable();

            caResult.AddListener(OnShotResult);
        }

        /// <inheritdoc/>
        protected override void OnMinigameDisable()
        {
            base.OnMinigameDisable();

            caResult.RemoveListener(OnShotResult);
        }


        /// <inheritdoc/>
        protected override void Initialize(ArgumentBuilder builder)
        {
            base.Initialize(builder);

            ServiceLocator.GetService<TrialStatsBase>().ShowTrialStats();

            activeStockPanel = GetComponentInChildren<IActiveStockPanel>();

            activeStockPanel.InitializeActivePanel(0);

            if (ServiceLocator.HasService<GlobalFade>())
                ServiceLocator.GetService<GlobalFade>().FadeOut(menuFadeWaitTime);

            introOutroUI.Show();

            inputAllowed.SetValue(true);
        }


        /// <inheritdoc/>
        protected override void StopGame()
        {
            base.StopGame();

            cameraUI.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void ContinueRestart(object o = null) => Restart();

        /// <inheritdoc/>
        protected override void Restart() => StartCoroutine(CAReset());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator CAReset()
        {
            cameraUI.Show();

            totalAnswered = 0;

            argumentReseters ??= FindObjectsByType<MonoBehaviour>().OfType<IClosingArgumentResetable>().ToArray();

            foreach (var listener in argumentReseters)
                listener.CAReset();

            argumentListeners = null;

            argumentReseters = null;

            yield return null;

            Initialize(builder);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="wasCorrect"></param>
        protected virtual void OnShotResult(bool wasCorrect) => StartCoroutine(wasCorrect ? CorrectAnim() : IncorrectAnim());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator CorrectAnim()
        {
            OnCorrect?.Invoke();

            yield return resultUI.ShowTillComplete();

            Transform hoveredObject = reticle.GetHoveredObject<MonoBehaviour>().transform;

            argumentListeners ??= FindObjectsByType<MonoBehaviour>()
                .OfType<IClosingArgumentListener>()
                .ToArray();

            foreach (var listener in argumentListeners)
                listener.OnStockCorrect(hoveredObject);

            totalAnswered++;

            if (totalStock == totalAnswered)
                StartCoroutine(ResolveMinigame());
            else
                inputAllowed.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator IncorrectAnim()
        {
            OnIncorrect?.Invoke();

            yield return resultUI.HideTillComplete();

            if (!trialTimeUp.Value)
                ServiceLocator.GetService<TrialStatsBase>().TakeDamage(damageTakenOnWrong);

            yield return null; // Buffer a frame to register whether the player has died.

            if (!playerHasDied.Value && !trialTimeUp.Value)
                inputAllowed.SetValue(true);
        }


        /// <inheritdoc/>
        public override IEnumerator ResolveMinigame()
        {
            inputAllowed.SetValue(false);

            TrialTimer.StopTimer();

            OnMinigameEnding?.Invoke();

            yield return new WaitForSeconds(menuFadeWaitTime);

            ServiceLocator.GetService<MusicManagerBase>().StopMusic();

            yield return answerUI.ShowTillComplete();

            if (ServiceLocator.HasService<GlobalFade>())
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(minigameEndFadeDuration);

            yield return base.ResolveMinigame();
        }
    }
}