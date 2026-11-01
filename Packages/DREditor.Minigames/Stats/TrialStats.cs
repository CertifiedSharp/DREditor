//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TrialStatsBase) +
        "\n\n")]
    public class TrialStats : TrialStatsBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMinigame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent statsUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected HealthStatBase healthStat = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float gameOverResetBufferTime = 2;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool visible = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool isTransitioning = false;


        protected virtual void OnEnable() => inTrialMinigame.AddListener(InMinigameUpdated);

        protected virtual void OnDisable() => inTrialMinigame.RemoveListener(InMinigameUpdated);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inMinigame"></param>
        protected virtual void InMinigameUpdated(bool inMinigame)
        {
            if (!inMinigame)
                HideTrialStats();
        }


        /// <inheritdoc/>
        public override void ShowTrialStats() => SetStatsVisibility(true);

        /// <inheritdoc/>
        public override void HideTrialStats() => SetStatsVisibility(false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetVisible"></param>
        protected virtual void SetStatsVisibility(bool targetVisible)
        {
            if (visible == targetVisible && !isTransitioning) 
                return;

            visible = targetVisible;

            StartCoroutine(TransitionProcess(targetVisible));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetVisible"></param>
        protected virtual IEnumerator TransitionProcess(bool targetVisible)
        {
            if (isTransitioning)
                yield return statsUI.WaitForChangeToComplete();

            isTransitioning = true;

            if (targetVisible)
                healthStat.Refresh();

            yield return targetVisible ? statsUI.ShowTillComplete() : statsUI.HideTillComplete();

            isTransitioning = false;
        }


        /// <inheritdoc/>
        public override void TakeDamage(float damage = 0)
        {
            ShowTrialStats();

            healthStat.TakeDamage(damage);
        }


        /// <inheritdoc/>
        public override IEnumerator GameOverResetProcess()
        {
            ShowTrialStats();

            healthStat.ResetHealth();

            yield return new WaitForSeconds(gameOverResetBufferTime);

            HideTrialStats();
        }

        /// <inheritdoc/>
        public override IEnumerator WaitForTweenToFinish()
        {
            if (isTransitioning)
                yield return statsUI.WaitForChangeToComplete();

            yield return new WaitUntil(() => !isTransitioning);
        }
    }
}