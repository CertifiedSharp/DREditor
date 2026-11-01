//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
#endif
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [ClassHeader(nameof(CounterType) +
        "\n\n")]
    public class CounterUIType : CounterType
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent counterVisibility = null;

#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [EventRef]
        [SerializeField] protected string protagCounterVO;
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip protagCounterVO;
#endif


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected EvaluateContextCondition<PhraseBase> evaluateCondition = null;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(UsingCondition))]
        [Tooltip("")]
        [SerializeField] protected bool conditionMustMatch = true;


        /// <summary>
        ///
        /// </summary>
        protected Canvas canvas = null;


        protected virtual void Awake()
        {
            if (TryGetComponent(out canvas))
                canvas.enabled = false;
        }


        /// <inheritdoc/>
        public override IEnumerator DisplayCounter(PhraseBase textPhrase, Line currentPanel)
        {
            counterVisibility.Show();

            canvas.enabled = true;

            ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(protagCounterVO);

            yield return counterVisibility.WaitForChangeToComplete();
        }

        /// <inheritdoc/>
        public override bool Evaluate(PhraseBase textPhrase) 
            => !UsingCondition() || evaluateCondition.Evaluate(textPhrase) == conditionMustMatch;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool UsingCondition() => evaluateCondition != null;
    }
}