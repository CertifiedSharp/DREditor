//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Minigames.Stats;
using DREditor.Services;
using DREditor.Investigations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DebateManagerBase<T> : MinigameManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool skipEarly = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DebateBuilder<T> debate = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent logicDifficulty = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent actionTimerDifficulty = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useCameraSmoothFocus = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool shuffleTruthBullets = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CursorLockMode cursorState = CursorLockMode.Confined;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float defaultTimeState = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float skipEarlyTimeState = 5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDebate = null;


        /// <summary>
        ///
        /// </summary>
        protected TrialStatsBase trialStats;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerator playPanelRoutine;


        protected virtual void Start()
        {
            trialStats = ServiceLocator.GetService<TrialStatsBase>();

            if (debate && debugMode)
                StartDebate(debate);
        }


        /// <summary>
        ///
        /// </summary>
        public virtual List<T> GetLines() => debate.Lines;

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        public void StartMinigame(ScriptableObject asset)
            => FindAnyObjectByType<DebateManagerBase<T>>().StartDebate((DebateBuilder<T>)asset);

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        public virtual void StartDebate(DebateBuilder<T> asset)
        {
            debate = asset;

            StartCoroutine(StartDebateRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator StartDebateRoutine();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTruthBullets()
        {
            List<TruthBullet> truthBullets = new();

            foreach(var bullet in debate.TruthBullets[logicDifficulty.Value].bullets)
                truthBullets.Add(bullet);

            if (shuffleTruthBullets)
                truthBullets.Shuffle();

            InvokeComponents<IBulletSendable>(sendable => sendable.OnSentBullets(truthBullets));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCountered()
        {
            if (!debugMode)
                StartCoroutine(ResolveMinigame());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDebate()
        {
            InitializeTruthBullets();

            Cursor.lockState = cursorState;

            SetTimeSpeed(skipEarlyTimeState);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveInitialization()
        {
            TrialTimer.Initialize(debate.times[actionTimerDifficulty.Value]);

            FindAnyObjectByType<DRTrialCamera>().SmoothFocus = useCameraSmoothFocus;

            SetTimeSpeed(defaultTimeState);

            InvokeComponents<IDebateUIVisibility>(visibility => visibility.Show());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="speed"></param>
        protected virtual void SetTimeSpeed(float speed)
        {
            if (skipEarly)
            {
                Time.timeScale = speed;
#if DOTWEEN
                DG.Tweening.DOTween.timeScale = speed;
#endif
            }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowUI()
        {
            TrialTimer.Show();

            TrialTimer.ResumeTimer();

            if (trialStats != null)
                trialStats.ShowTrialStats();

            InvokeComponents<IDebateUIVisibility>(visibility => visibility.Show());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideUI()
        {
            TrialTimer.Hide();

            TrialTimer.PauseTimer();

            if (trialStats != null)
                trialStats.HideTrialStats();

            InvokeComponents<IDebateUIVisibility>(visibility => visibility.Hide());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartPanels()
        {
            inDebate.SetValue(true);

            inputAllowed.SetValue(true);

            playPanelRoutine = PlayPanels();

            StartCoroutine(playPanelRoutine);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator PlayPanels();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPanelsResolved()
        {
            HideUI();

            inputAllowed.SetValue(false);

            InvokeComponents<IPanelsResolvedListener>(listener => listener.OnPanelsResolved());
        }


        protected override void Restart()
        {
            StopAllCoroutines();

            if (!gameObject.activeSelf)
                return;

            ShowUI();

            StartPanels();
        }
    }
}