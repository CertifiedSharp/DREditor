//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class StartInvestigationAnimation : PhaseStartAnimationHandler<StartInvestigationAnimation>
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
        [SerializeField] protected IntWithEvent currentState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int currentStateValue = 2;


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
        [SerializeField] protected int currentAddStateValue = 1;


        /// <inheritdoc/>
        protected override void PlayStartAnimation() => startAnimator.SetTrigger(startTriggerName);

        /// <inheritdoc/>
        public override void AfterLoadingRoom()
        {
            currentState.SetValue(currentStateValue);

            currentAddState.SetValue(currentAddStateValue);
        }

        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public virtual void DebugStartAnimation() => StartCoroutine(OnPhaseStart(null));
    }
}