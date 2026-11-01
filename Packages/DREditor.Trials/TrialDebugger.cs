//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Minigames;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class TrialDebugger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float debugInitializeWaitTime = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool startLastIndex = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int debugIndex = 0;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TrialManagerBase manager;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TrialSequencer sequencer;


        protected virtual void Start()
        {
            if (debugMode)
                StartCoroutine(Debugging());
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual IEnumerator Debugging()
        {
            UnityEngine.Debug.LogWarning("REPORT: Trial is currently in debug mode, please ensure the trial manager does " +
                "not have \"Play On Start\" enabled! Otherwise this will cause issues in debugging.");

            yield return new WaitForSecondsRealtime(debugInitializeWaitTime);

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

            if (startLastIndex)
                manager.StartTrialAtChapter();
            else
                manager.StartTrialAtIndex(debugIndex);
        }


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Skip Trial Sequence")]
        public void DebugSkipTrialSequence() => SkipTrialSequence();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SkipTrialSequence()
        {
            ServiceLocator.GetService<IDialogueManager>().StopAllCoroutines();

            StopAllCoroutines();

            if (debugIndex == 0)
                sequencer.SetTrialSequenceFinished(true);
            else
                StartCoroutine(Debugging());
        }
    }
}