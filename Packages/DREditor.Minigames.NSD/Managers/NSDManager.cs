//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Managers;
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.NSD.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DebateManagerBase<Panel>) + ", " + nameof(MinigameManagerBase) + ", " +
        "\n\n")]
    public class NSDManager : DebateManager<Panel>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent onDebateIntroResolved;

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
        protected IDebateLineManager lineManager = null;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            lineManager = GetComponentInChildren<IDebateLineManager>();
        }

        /// <inheritdoc/>
        protected override IEnumerator StartDebateRoutine()
        {
            InitializeDebate();

            ServiceLocator.GetService<MusicManagerBase>().PlayMusic(debate.StartingMusic);

            if (introVisibility != null)
                yield return introVisibility.ShowTillComplete();

            if (loadBulletVisibility != null)
                yield return loadBulletVisibility.ShowTillComplete();

            ResolveInitialization();

            StartPanels();
        }

        /// <inheritdoc/>
        protected override void ResolveInitialization()
        {
            InvokeComponents<IPanelCounter>(counter => counter.InitializePanelCount(debate.GetPanelCount()));

            base.ResolveInitialization();

            onDebateIntroResolved.Raise();

            if (trialStats != null)
                trialStats.ShowTrialStats();
        }
        
        /// <inheritdoc/>
        protected override IEnumerator PlayPanels()
        {
            inMenu.Invoke();

            yield return lineManager.PlayLines(debate.Lines);

            OnPanelsResolved();
        }

        /// <inheritdoc/>
        protected override void OnPanelsResolved()
        {
            base.OnPanelsResolved();

            if (debate is NSDBuilder builder)
                ServiceLocator.GetService<IDialogueManager>().StartDialogue(null, (TrialDialogue)builder.EndDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public void ShowLoadBulletAnimation()
        {
            if (!Application.isPlaying)
                return;

            InitializeDebate();

            if (loadBulletVisibility != null)
                loadBulletVisibility.Show();
        }
    }
}