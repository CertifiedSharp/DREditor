//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using DREditor.UI;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TrialPrepUIBase) + ", " + nameof(SceneEventListener) +
        "\n\n")]
    public class TrialPrepUI : TrialPrepUIBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentAddState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent screenIsDark = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Button startPauseButton = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Button startTrialButton = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroup trialPrepGroup = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent trialStartAnimation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float animationFadeTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool skipFadeOut = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int trialPrepAddState = 2;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int startTrialAddState = 0;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            SubscribeToButtons();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToButtons()
        {
            startPauseButton.onClick.AddListener(ShowPause);

            startTrialButton.onClick.AddListener(BeginTrial);
        }


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            UnsubscribeToButtons();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToButtons()
        {
            startPauseButton.onClick.RemoveAllListeners();

            startTrialButton.onClick.RemoveAllListeners();
        }


        /// <inheritdoc/>
        protected override void Initialize()
        {
            base.Initialize();

            currentAddState.SetValue(trialPrepAddState);
        }

        /// <inheritdoc/>
        protected override IEnumerator InitializePreparation()
        {
            if (!ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeTo(0);

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

            if (isLoadingFile.Value)
                yield return InitializePreparationFromSaveFile();

            yield return new WaitUntil(() => !inDialogue.Value);

            InitializeTrialState();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator InitializePreparationFromSaveFile()
        {
            yield return new WaitUntil(() => ServiceLocator.GetService<GlobalFade>().isTransitioning);

            yield return new WaitUntil(() => !isLoadingFile.Value);

            trialPrepGroup.Show();
        }


        /// <inheritdoc/>
        public override void ShowPause()
        {
            if (screenIsDark.Value)
                return;

            EventSystem.current.SetSelectedGameObject(null);

            trialPrepGroup.EnableSelectables(false);

            startPauseGroup.Raise();
        }


        /// <inheritdoc/>
        public override void OnResumeFinished()
        {
            trialPrepGroup.EnableSelectables(true);

            trialPrepGroup.SetEvaluateSelect();
        }

        /// <inheritdoc/>
        public override void BeginTrial()
        {
            EventSystem.current.SetSelectedGameObject(null);

            currentAddState.SetValue(startTrialAddState);

            StartCoroutine(BeginTrialRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BeginTrialRoutine()
        {
            ServiceLocator.GetService<MusicManagerBase>().StopPlayingMusic();

            if (trialStartAnimation != null)
                yield return trialStartAnimation.ShowTillComplete();

            if (!skipFadeOut)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(animationFadeTime);
            else
                ServiceLocator.GetService<GlobalFade>().FadeTo(0);

            UnityEngine.Debug.LogWarning("REPORT: CHANGING SCENE");

            SceneManager.LoadSceneAsync(trialSceneName);
        }
    }
}