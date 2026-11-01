//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PhaseStartAnimationHandler<T> : PhaseStartHandler<T>, IPhaseStartHandler where T : class
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected UnityEvent onAnimationStarted;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected Canvas startCanvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Animator startAnimator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int animationLayer = 0;


        public override IEnumerator OnPhaseStart(StartInvestigation start)
        {
            PlayPhaseStartAnimation();

            yield return startAnimator.WaitForStateToComplete(animationLayer);

            startCanvas.enabled = false;

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlayPhaseStartAnimation()
        {
            if (startAnimator != null)
                PlayStartAnimation();
            else
                UnityEngine.Debug.Log($"{ToString()}: REPORT: The Phase Start Animator was not found! ");

            startCanvas.enabled = true;

            onAnimationStarted.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void PlayStartAnimation();
    }
}