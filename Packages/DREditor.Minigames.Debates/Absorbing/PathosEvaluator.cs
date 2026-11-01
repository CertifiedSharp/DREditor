//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Minigames.Debates.Counters;
using DREditor.Minigames.Debates.Mechanics;
using UnityEngine;

namespace DREditor.Minigames.Debates.Absorbing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DebateEvaluator) +
        "\n\n")]
    public class PathosEvaluator : DebateEvaluator
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent skipTrialSequences = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inEmpathy = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent bulletText = null;


        /// <summary>
        ///
        /// </summary>
        protected IPathosAnswer modifier;

        /// <summary>
        ///
        /// </summary>
        protected IBulletEvaluatorManager bulletEvaluatorManager;


        protected virtual void Awake()
        {
            if (!TryGetComponent(out bulletEvaluatorManager))
                bulletEvaluatorManager = transform.root.GetComponentInChildren<IBulletEvaluatorManager>();
        }

        protected virtual void OnEnable() => bulletEvaluatorManager.OnCounterFinished += OnCounterFinished;

        protected virtual void OnDisable() => bulletEvaluatorManager.OnCounterFinished -= OnCounterFinished;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCounterFinished()
        {
            if (modifier != null && bulletText.Value == modifier.PathosAnswer && inEmpathy.Value)
                skipTrialSequences.SetValue(modifier.PathosSequenceSkips);
        }


        /// <inheritdoc/>
        public override bool Evaluate(IBulletSwapperUI bulletHandler, PhraseBase textPhrase) 
            => bulletHandler != null && inEmpathy.Value && textPhrase.Has(out modifier) && bulletText.Value == modifier.PathosAnswer;
    }
}