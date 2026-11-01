//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class TrialSequencer : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent skipTrialSequences = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent hideTrialDialogueUI = null;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MinigameType trialDiscussionType = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TrialEnderBase trialEnder = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool onlyFirstSequence = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int currentIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string endMinigameEvent = "EndMinigame";


        /// <summary>
        ///
        /// </summary>
        protected bool sequenceFinished = false;

        /// <summary>
        ///
        /// </summary>
        protected int skip = 0;

        /// <summary>
        ///
        /// </summary>
        protected TrialBuilder trial;

        /// <summary>
        ///
        /// </summary>
        protected TrialSequence currentSequence;


        protected virtual void OnEnable()
        {
            DialogueEventSystem.StartListening(endMinigameEvent, EndTrialSequence);

            if (skipTrialSequences != null)
                skipTrialSequences.AddListener(SkipSequences);
        }

        protected virtual void OnDisable()
        {
            DialogueEventSystem.StopListening(endMinigameEvent, EndTrialSequence);

            if (skipTrialSequences != null)
                skipTrialSequences.RemoveListener(SkipSequences);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="sequenceFinished"></param>
        public virtual void SetTrialSequenceFinished(bool sequenceFinished) => this.sequenceFinished = sequenceFinished;

        /// <summary>
        ///
        /// </summary>
        /// <param name="trial"></param>
        /// <param name="startIndex"></param>
        public virtual IEnumerator StartTrial(TrialBuilder trial, int startIndex = 0)
        {
            this.trial = trial;

            UnityEngine.Debug.Log("Starting Trial: " + trial.name);

            for (currentIndex = startIndex; currentIndex < trial.sequences.Count; currentIndex++)
            {
                CheckSkipSequences();

                yield return InitializeSequence();

                if (onlyFirstSequence && currentIndex == 0)
                    break;
            }

            StartCoroutine(trialEnder.EndTrial(trial));

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckSkipSequences()
        {
            if (!(skip + currentIndex < trial.sequences.Count))
                UnityEngine.Debug.LogWarning("ALERT: You are skipping more sequences than are available. Skipping aborted.");
            else if (skip > 0 && skip + currentIndex < trial.sequences.Count)
            {
                UnityEngine.Debug.Log("Skipping sequences at: " + currentIndex + " to: " + (skip + currentIndex));

                currentIndex += skip;

                skip = 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator InitializeSequence()
        {
            currentSequence = trial.sequences[currentIndex];

            InitializeMinigameManager();

            SetMinigameControls();

            while (!sequenceFinished)
                yield return null;

            ResolveDialogueUI();

            EndTrialSequence();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeMinigameManager()
        {
            GameObject minigamePrefab = GameObject.Find(currentSequence.MinigameType.Prefab.name);

            if (minigamePrefab == null)
                Instantiate(currentSequence.MinigameType.Prefab);

            currentSequence.MinigameType.manager.Invoke(currentSequence.MinigameAsset);

            UnityEngine.Debug.Log("Started Minigame: " + currentSequence.MinigameType.TypeName);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetMinigameControls()
        {
            if (currentSequence.MinigameType.TypeName != trialDiscussionType.TypeName)
                currentControlsKey.SetValue(currentSequence.MinigameType.ControlsKey);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveDialogueUI()
        {
            if (currentIndex + 1 >= trial.sequences.Count)
                return;

            bool currentSequenceIsDialogue = currentSequence.MinigameType.TypeName == trialDiscussionType.TypeName;

            bool nextSequenceNotDialogue =
                trial.sequences[currentIndex + 1].MinigameType.TypeName != trialDiscussionType.TypeName;

            if (currentSequenceIsDialogue && nextSequenceNotDialogue)
                hideTrialDialogueUI.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EndTrialSequence(object o = null) => sequenceFinished = !sequenceFinished;

        /// <summary>
        ///
        /// </summary>
        /// <param name="skipNum"></param>
        protected virtual void SkipSequences(int skipNum)
        {
            if (skipNum > 0)
            {
                skip = skipNum;

                skipTrialSequences.SetValue(0);
            }
        }
    }
}