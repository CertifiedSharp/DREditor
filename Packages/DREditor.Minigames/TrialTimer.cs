//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TrialTimerBase) + ", " + nameof(TimerBase) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class TrialTimer : TrialTimerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string resetTimerText = "00:00.000";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string timerFormat = "{0:00}:{1:00}.{2:000}";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent = null;


        /// <summary>
        ///
        /// </summary>
        protected TMP_Text timerText = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        protected virtual void SetTimerText(string text) => timerText.text = text;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (!TryGetComponent(out timerText))
                if (GetComponentInChildren<TMP_Text>() is TMP_Text childText)
                    timerText = childText;
                else
                    UnityEngine.Debug.LogError(nameof(TrialTimer) + ": Could not find a TMP text component on the timer!");
        }


        /// <inheritdoc/>
        public override void StopTimer()
        {
            base.StopTimer();

            SetTimerText("");
        }

        /// <inheritdoc/>
        protected override void CountDownTime()
        {
            base.CountDownTime();

            if (minutes < 0 || seconds < 0 || milliseconds < 0)
                SetTimeRemaining(-1);
            else
                SetTimerVisual();
        }

        /// <inheritdoc/>
        protected override void TimeHasRunOut()
        {
            SetTimerText(resetTimerText);

            base.TimeHasRunOut();
        }


        /// <inheritdoc/>
        public override void Show()
        {
            if (visibilityComponent != null)
                visibilityComponent.Show();
        }

        /// <inheritdoc/>
        public override void Hide()
        {
            if (visibilityComponent != null)
                visibilityComponent.Hide();
        }

        /// <inheritdoc/>
        public override IEnumerator WaitForChangeToComplete()
        {
            if (visibilityComponent != null)
                yield return visibilityComponent.WaitForChangeToComplete();
        }

        /// <inheritdoc/>
        protected override void SetTimerVisual()
            => SetTimerText(string.Format(timerFormat, minutes, seconds, milliseconds));
    }
}