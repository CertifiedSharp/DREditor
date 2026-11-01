//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Minigames.Debates.Managers;
using DREditor.Processables;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DebateManager<RSGroup>) + 
        "\n\n")]
    public class RSManager : DebateManager<RSGroup>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent showRSUI;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float loadBufferTime = 1.0f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] private MultiTrialDialogue debugStartDialogue = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InterjectHandlerBase interjectionHandler;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent introVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent loadBulletVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InitializableBase multiPanelManager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueComponent multiLineProcessor;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float fadeToTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endFadeToTime = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endFadeOutTime = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected bool introDialogueActive = false;

        /// <summary>
        ///
        /// </summary>
        protected IDebateGroupManager groupManager = null;

        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager manager = null;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            manager ??= GetComponentInChildren<IDialogueManager>();
        }

        /// <inheritdoc/>
        protected override IEnumerator StartDebateRoutine()
        {
            if (isLoadingFile.Value)
                yield return new WaitForSeconds(loadBufferTime);

            InitializeDebate();

            yield return InterjectionProcess();

            yield return StartingDialogueProcess();

            yield return IntroduceShowdown();

            ResolveInitialization();

            StartPanels();
        }

        /// <inheritdoc/>
        protected override void InitializeDebate()
        {
            if (ServiceLocator.HasService<IDebateGroupManager>())
                groupManager = ServiceLocator.GetService<IDebateGroupManager>();
            else
                UnityEngine.Debug.LogError("ALERT: A " + nameof(IDebateGroupManager) + " service must be available in the scene!");

            groupManager?.OnPanelsResolved.AddListener(OnPanelsResolved);

            groupManager?.OnRestartPanels.AddListener(Restart);

            groupManager?.OnStopPanels.AddListener(StopPanels);

            base.InitializeDebate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator InterjectionProcess()
        {
            if (debate is RSBuilder builder && builder.interjectObject != null && interjectionHandler != null)
                yield return interjectionHandler.InterjectionProcess(builder.interjectObject, skipEarly);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator StartingDialogueProcess()
        {
            if (skipEarly)
                yield break;

            MultiTrialDialogue dialogue = null;

            if (debate is RSBuilder builder)
                dialogue = builder.StartingRSDialogue;
            else if (debugStartDialogue != null)
                dialogue = debugStartDialogue;

            if (dialogue != null)
            {
                introDialogueActive = true;

                if (isLoadingFile.Value)
                    multiPanelManager.Initialize();

                dialogue.InvokeDialogue(ServiceLocator.GetService<IDialogueManager>());

                yield return new WaitUntil(() => inDialogue.Value);

                yield return new WaitUntil(() => !inDialogue.Value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator IntroduceShowdown()
        {
            ServiceLocator.GetService<MusicManagerBase>().PlayMusic(debate.StartingMusic);

            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeToTime);

            ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutTime);

            if (showRSUI != null)
                showRSUI.Raise();

            DisplayIntroPanels();

            if (introVisibility != null)
                yield return introVisibility.ShowTillComplete();

            if (trialStats != null)
                trialStats.ShowTrialStats();

            if (loadBulletVisibility != null)
                yield return loadBulletVisibility.ShowTillComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayIntroPanels()
        {
            if (skipEarly)
                multiPanelManager.Initialize();

            if (debate is RSBuilder builder && builder.introPanels.Count > 0 &&
                multiLineProcessor is ILineProcessor lineProcessor)
                lineProcessor?.LineProcess(new MultiTrialLine(builder.introPanels));
        }

        /// <inheritdoc/>
        protected override IEnumerator PlayPanels()
        {
            inMenu.Invoke();

            if (debate is RSBuilder builder)
                yield return groupManager?.PlayGroups(builder);
        }

        /// <inheritdoc/>
        protected override void Restart()
        {
            if (introDialogueActive)
            {
                introDialogueActive = false;

                return;
            }

            base.Restart();
        }

        /// <inheritdoc/>
        protected override void OnMinigameDisable()
        {
            base.OnMinigameDisable();

            groupManager?.OnPanelsResolved.RemoveListener(OnPanelsResolved);

            groupManager?.OnRestartPanels.RemoveListener(Restart);

            groupManager?.OnStopPanels.RemoveListener(StopPanels);
        }

        /// <inheritdoc/>
        public override IEnumerator ResolveMinigame()
        {
            Destroy(manager.GetManagerObject().transform.parent.gameObject);

            if (ServiceLocator.HasService(out GlobalFade fader))
                yield return fader.FadeToRoutine(endFadeToTime);

            yield return new WaitUntil(() => (manager as Component) == null);
            
            yield return base.ResolveMinigame();
        }

        protected virtual void OnDestroy()
        {
            if (ServiceLocator.HasService(out UIHandlerBase uiHandler) && !uiHandler.MovingToTitleScreen && 
                ServiceLocator.HasService(out GlobalFade fader))
                fader.FadeOut(endFadeOutTime);
        }


        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public void IntroPanelTest()
        {
            if (Application.isPlaying)
                DisplayIntroPanels();
        }
    }
}