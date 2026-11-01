//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MinigameManagerBase) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class TrialDialogueMinigame : MinigameManagerBase, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialContinued = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TrialDialogue continueConfirm = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TrialDialogue timeUpDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int setActiveBufferFrames = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string onTrialResolving = "ResolveTrial";


        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager manager = null;

        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager originalManager = null;

        /// <summary>
        ///
        /// </summary>
        protected bool originalManagerRestored = false;


        protected virtual void Awake()
        {
            manager = GetComponentInChildren<IDialogueManager>();

            StartCoroutine(BufferSetActiveManager());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferSetActiveManager()
        {
            yield return new WaitForSeconds(Time.deltaTime * setActiveBufferFrames);

            SetActiveManager();
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            manager ??= GetComponentInChildren<IDialogueManager>();

            manager.GetFinisher().SubscribeToOnFinishOfDialogue(EvaluateFinishedDialogue);

            trialContinued.AddListener(OnTrialContinue);

            trialTimeUp.AddListener(OnTrialTimeUp);

            DialogueEventSystem.StartListening(onTrialResolving, EvalutateRestoreOriginalManager);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            manager?.GetFinisher().UnsubscribeToOnFinishOfDialogue(EvaluateFinishedDialogue);

            ToTitleScreen();

            trialContinued.RemoveListener(OnTrialContinue);

            trialTimeUp.RemoveListener(OnTrialTimeUp);

            DialogueEventSystem.StopListening(onTrialResolving, EvalutateRestoreOriginalManager);

            trialMistake.SetValue(false);

            inputAllowed.SetValue(false);

            playerHasDied.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public void StartMinigame(ScriptableObject o) => FindAnyObjectByType<TrialDialogueMinigame>().StartDialogueMinigame(o);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public virtual void StartDialogueMinigame(ScriptableObject o)
        {
            UnityEngine.Debug.Log("REPORT: Started Trial Discussion.");

            manager.StartDialogue(null, ((TrialDiscussion)o).trialDialogues);

            SetActiveManager();

            inTrialMinigame.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetActiveManager()
        {
            if (!ServiceLocator.HasService(out IDialogueManager currentManager))
            {
                manager.GetManagerComponent<DialogueDataClearer>().ClearSaveData();

                ServiceLocator.Register(manager);
            }
            else if (currentManager != manager)
            {
                originalManager = currentManager;

                originalManager.GetManagerComponent<DialogueDataClearer>().ClearSaveData();

                ServiceLocator.Unregister(originalManager);

                ServiceLocator.Register(manager);

                UnityEngine.Debug.Log($"REPORT: {gameObject.name} found Original Dialogue Manager: " + (originalManager as Component).name);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void EvalutateRestoreOriginalManager(object o = null) => ToTitleScreen();

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator QueueRestoringManagerRoutine()
        {
            UnityEngine.Debug.LogWarning($"{ToString()} The current manager is not the one currently assigned or the original." +
                $" Waiting for assigned manager to be restored.");

            yield return new WaitUntil(() => ServiceLocator.GetService<IDialogueManager>() == manager);

            RestoreOriginalManager();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RestoreOriginalManager()
        {
            ServiceLocator.Unregister(manager);

            ServiceLocator.Register(originalManager);

            UnityEngine.Debug.Log("REPORT: Original Dialogue Manager restored to: " + (originalManager as Component).name);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateFinishedDialogue()
        {
            trialTimeUp.SetValue(false);

            if (trialMistake.Value && !playerHasDied.Value)
                FinishedTrialMistake();
            else if (playerHasDied.Value)
                OnPlayerDied();
            else if (inTrialMinigame != null && inTrialMinigame.Value)
                FinishedTrialMistake();
            else
                EndMinigame();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="playerIsContinuing"></param>
        protected virtual void OnTrialContinue(bool playerIsContinuing)
        {
            if (playerIsContinuing && ServiceLocator.GetService<IDialogueManager>() == manager)
                manager.StartDialogue(null, continueConfirm);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="minigameTimeUp"></param>
        protected virtual void OnTrialTimeUp(bool minigameTimeUp)
        {
            if (minigameTimeUp && ServiceLocator.GetService<IDialogueManager>() == manager)
                manager.StartDialogue(null, timeUpDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPlayerDied()
        {
            if (trialContinued.Value)
            {
                trialContinued.SetValue(false);

                playerHasDied.SetValue(false);

                FinishedTrialMistake();
            }
        }

        /// <inheritdoc/>
        protected override void Restart() { }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            if (originalManagerRestored)
                return;
            else
                originalManagerRestored = true;

            DialogueEventSystem.StopListening(onTrialResolving, EvalutateRestoreOriginalManager);

            if ((originalManager as Component) == null)
                UnityEngine.Debug.Log($"{gameObject.name} " +
                    "REPORT: There was no original service.");
            else if (!ServiceLocator.HasService<IDialogueManager>(out var currentManager) || currentManager == manager)
                RestoreOriginalManager();
            else if (!ReferenceEquals(currentManager, originalManager))
                StartCoroutine(QueueRestoringManagerRoutine());
            else
                UnityEngine.Debug.Log($"{gameObject.name} " +
                    $"REPORT: Current Dialogue Manager is original: " + (currentManager as Component).name);
        }
    }
}