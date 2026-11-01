//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TrialTimerBase : TimerBase, IUIVisibility
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent actionTimerDifficulty = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="times"></param>
        /// <param name="finishVisualStart"></param>
        public virtual void ShowInitialize(List<TimerDifficulty> times, bool finishVisualStart = false)
        {
            Initialize(times);

            Show();

            StartCoroutine(ResumeOnVisualEnd(finishVisualStart));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="times"></param>
        public virtual void Initialize(List<TimerDifficulty> times)
        {
            TimerDifficulty timed = times[actionTimerDifficulty.Value];

            float time = (timed.minutes * 60) + timed.seconds;

            StartTimer(time);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="finishVisualOnStart"></param>
        /// <returns></returns>
        protected virtual IEnumerator ResumeOnVisualEnd(bool finishVisualOnStart)
        {
            PauseTimer();

            ResetTimer();

            if (finishVisualOnStart)
                yield return WaitForChangeToComplete();

            ResumeTimer();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void HideStop()
        {
            Hide();

            StopTimer();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void DisplayRemainingTime()
        {
            CalculateTimerVariables();

            SetTimerVisual();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void SetTimerVisual();


        /// <inheritdoc/>
        public abstract void Show();

        /// <inheritdoc/>
        public abstract void Hide();

        /// <inheritdoc/>
        public abstract IEnumerator WaitForChangeToComplete();

        /// <summary>
        ///
        /// </summary>
        /// <param name="timed"></param>
        public virtual void Initialize(TimerDifficulty timed)
        {
            SetTimer(timed);

            Show();

            PauseTimer();

            ResetTimer();

            ResumeTimer();
        }


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Debug Start")]
        public void DebugStart()
        {
            Show();

            StartTimer(startTime);
        }
    }
}