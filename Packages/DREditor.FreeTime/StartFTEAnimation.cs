//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving.Handlers;
using DREditor.DialogueMechanics.Investigation;
using DREditor.Dialogues;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPhaseStartHandler) +
        "\n\n")]
    public class StartFTEAnimation : PhaseStartAnimationHandler<StartFTEAnimation>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string startTriggerName = "Start";

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected int addCurrencyEachDay = 10;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CurrencyHandler<int> currencyHandler = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentAddState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int currentAddStateValue = 5;

        

        /// <inheritdoc/>
        public override IEnumerator OnPhaseStart(StartInvestigation start)
        {
            yield return base.OnPhaseStart(start);

            currencyHandler.AddCurrency(addCurrencyEachDay);
        }

        /// <inheritdoc/>
        protected override void PlayStartAnimation() => startAnimator.SetTrigger(startTriggerName);

        /// <inheritdoc/>
        public override void AfterLoadingRoom() => currentAddState.SetValue(currentAddStateValue);

        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public virtual void DebugStartAnimation() => StartCoroutine(OnPhaseStart(null));
    }
}