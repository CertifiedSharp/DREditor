//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TimerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startTime = 2 * 60;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float timeRemaining;


        /// <summary>
        ///
        /// </summary>
        public event Action TimeUp;


        /// <summary>
        ///
        /// </summary>
        protected bool timerIsOn = false;

        /// <summary>
        ///
        /// </summary>
        protected float minutes;

        /// <summary>
        ///
        /// </summary>
        protected float seconds;

        /// <summary>
        ///
        /// </summary>
        protected float milliseconds;


        /// <summary>
        ///
        /// </summary>
        /// <param name="timerIsOn"></param>
        protected virtual void SetTimerActive(bool timerIsOn) => this.timerIsOn = timerIsOn;

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeRemaining"></param>
        protected virtual void SetTimeRemaining(float timeRemaining) => this.timeRemaining = timeRemaining;

        /// <summary>
        ///
        /// </summary>
        protected virtual void CalculateTimerVariables()
        {
            minutes = Mathf.FloorToInt(timeRemaining / 60);

            seconds = Mathf.FloorToInt(timeRemaining % 60);

            milliseconds = (float)Math.Round(timeRemaining % 1 * 1000, 3);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Update()
        {
            if (timerIsOn)
                UpdateTimer();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateTimer()
        {
            if (timeRemaining >= 0)
                CountDownTime();
            else
                TimeHasRunOut();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CountDownTime()
        {
            SetTimeRemaining(timeRemaining - Time.unscaledDeltaTime);

            CalculateTimerVariables();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TimeHasRunOut()
        {
            UnityEngine.Debug.Log("REPORT: Time has run out!");

            SetTimeRemaining(0);

            SetTimerActive(false);

            TimeUp?.Invoke();

            ResetTimer();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="timeToAdd"></param>
        public virtual void AddTime(float timeToAdd) => SetTimeRemaining(timeRemaining + timeToAdd);

        /// <summary>
        ///
        /// </summary>
        /// <param name="lostTime"></param>
        public virtual void RemoveTime(float lostTime) => SetTimeRemaining(timeRemaining - lostTime);

        /// <summary>
        ///
        /// </summary>
        /// <param name="startingTime"></param>
        public virtual void StartTimer(float startingTime = 2 * 60)
        {
            startTime = startingTime;

            ResetTimer();

            SetTimerActive(true);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void StopTimer() => SetTimerActive(false);

        /// <summary>
        ///
        /// </summary>
        public virtual void PauseTimer() => SetTimerActive(false);

        /// <summary>
        ///
        /// </summary>
        public virtual void ResumeTimer() => SetTimerActive(timeRemaining > 0);

        /// <summary>
        ///
        /// </summary>
        public virtual void ResetTimer() => SetTimeRemaining(startTime);

        /// <summary>
        ///
        /// </summary>
        public virtual void Restart()
        {
            ResetTimer();

            ResumeTimer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timed"></param>
        public virtual void SetTimer(TimerDifficulty timed)
        {
            float time = (timed.minutes * 60) + timed.seconds;

            StartTimer(time);
        }
    }
}