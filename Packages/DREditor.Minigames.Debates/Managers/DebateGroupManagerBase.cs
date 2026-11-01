//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DebateGroupManagerBase<T> : ServiceHolder<IDebateGroupManager>, IDebateGroupManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int stallNumber = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int winNumber = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int loseNumber = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int tieNumber = -1;


        [Header("")]
        [SerializeField] protected UnityEvent onPanelsResolved;

        [Header("")]
        [SerializeField] protected UnityEvent<DialogueCore> onStopPanels;

        [Header("")]
        [SerializeField] protected UnityEvent onRestartPanels;


        public UnityEvent OnPanelsResolved { get => onPanelsResolved; }

        public UnityEvent<DialogueCore> OnStopPanels { get => onStopPanels; }

        public UnityEvent OnRestartPanels { get => onRestartPanels; }


        /// <summary>
        ///
        /// </summary>
        protected IDebateSequence debate = null;

        /// <summary>
        ///
        /// </summary>
        protected T currentGroup;

        /// <summary>
        ///
        /// </summary>
        protected int currentGroupIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected int debateLineCount = -1;


        public virtual IEnumerator PlayGroups<T2>(DialogueBase<T2> debate)
        {
            if (debate is not IDebateSequence debateSequence)
            {
                UnityEngine.Debug.LogError($"ALERT: Debate must implement {nameof(IDebateSequence)}.");

                yield break;
            }

            if (currentGroupIndex < 0 || currentGroupIndex >= debate.LineCount)
            {
                UnityEngine.Debug.LogError($"ALERT: Invalid group index: {currentGroupIndex}.");

                yield break;
            }

            if (debate.Lines[currentGroupIndex] is not T group)
            {
                UnityEngine.Debug.LogError($"ALERT: Line at index {currentGroupIndex} is not an {typeof(T).Name}.");

                yield break;
            }

            this.debate = debateSequence;

            currentGroup = group;

            debateLineCount = debate.LineCount;

            yield return PlayValidGroups();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator PlayValidGroups();

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator EvaluateSectionResult();

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator Stall();

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator Advance();

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator Retreat();
    }
}