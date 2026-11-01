//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TrialManagerBase) + ", " + nameof(IDialogueHolder) +
        "\n\n")]
    public class TrialManager : TrialManagerBase, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingToRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEventListener LoadTrialIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedLoadingRoomNoDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TrialSequencer sequencer = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool playOnStart = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<TrialBuilder> trials = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float enterFadeOutBuffer = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float enterFadeOutDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> initializeDialogueEvents = new() { "DisablePlayer" };


        /// <summary>
        ///
        /// </summary>
        protected int loadTrialIndex = -1;


        protected virtual void Awake() => DialogueEventSystem.TriggerEvents(initializeDialogueEvents);

        protected virtual void OnEnable()
        {
            if (inTrialMode != null && !inTrialMode.Value)
                inTrialMode.SetValue(true);

            if (LoadTrialIndex != null)
                LoadTrialIndex.Response.AddListener(LoadTrialAtDialogueIndex);
        }

        protected virtual void OnDisable()
        {
            if (inTrialMode != null)
                inTrialMode.SetValue(false);

            if (LoadTrialIndex != null)
                LoadTrialIndex.Response.RemoveListener(LoadTrialAtDialogueIndex);

            onFinishedLoadingRoomNoDialogue.Response.RemoveListener(InitializeTrial);
        }

        protected virtual void Start()
        {
            if (playOnStart)
                if (isLoadingToRoom.Value)
                    onFinishedLoadingRoomNoDialogue.Response.AddListener(InitializeTrial);
                else
                    InitializeTrial();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTrial()
        {
            onFinishedLoadingRoomNoDialogue.Response.RemoveListener(InitializeTrial);

            if (isLoadingFile.Value)
                return;

            if (currentChapterIndex.Value < trials.Count &&
                    trials[currentChapterIndex.Value] != null)
                StartCoroutine(sequencer.StartTrial(trials[currentChapterIndex.Value]));
            else
                UnityEngine.Debug.LogError("ALERT: Current chapter does not have a trial asset.");

            StartCoroutine(BufferFadeOut());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferFadeOut()
        {
            yield return new WaitForSeconds(enterFadeOutBuffer);

            if (ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeOut(enterFadeOutDuration);
        }


        /// <inheritdoc/>
        protected override void LoadTrialAtDialogueIndex()
        {
            UnityEngine.Debug.Log("REPORT: Getting Trial for Ch: " + currentChapterIndex.Value);

            int loadIndex = EvaluateLoadIndex();

            StartTrialAtIndex(loadIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int EvaluateLoadIndex()
        {
            var result = loadTrialIndex;

            loadTrialIndex = -1;

            if (result == -1)
                UnityEngine.Debug.LogWarning("ALERT: LOADING TRIAL AT DIALOGUE INDEX COULD NOT FIND AN INDEX!");

            return result;
        }

        /// <inheritdoc/>
        public override void StartTrialAtIndex(int startIndex)
        {
            UnityEngine.Debug.Log("REPORT: Starting Trial at Index: " + startIndex);

            StartCoroutine(sequencer.StartTrial(trials[currentChapterIndex.Value], startIndex));
        }

        /// <inheritdoc/>
        public override void StartTrialAtChapter() => StartTrialAtIndex(trials[currentChapterIndex.Value].sequences.Count - 1);

        
        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
        {
            var sequences = trials[currentChapterIndex.Value].sequences;

            var loadIdentifier = ServiceLocator.GetService<IDialogueManager>()?.GetManagerComponent<DialogueLoadIdentifier>();

            loadTrialIndex = sequences.FindIndex(sequence =>
                (sequence.MinigameAsset is IDialogueHolder holder && holder.CheckIfSaveDialogue()) ||
                (sequence.MinigameAsset is TrialDialogue dialogue && loadIdentifier != null && loadIdentifier.CheckDialogue(dialogue))
            );

            return loadTrialIndex != -1;
        }
    }
}