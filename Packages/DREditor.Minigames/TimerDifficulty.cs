//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine.Serialization;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class TimerDifficulty
    {
        /// <summary>
        ///
        /// </summary>
        public int difficulty = 0;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("min")]
        public float minutes = 1;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("sec")]
        public float seconds;


        public TimerDifficulty(int difficulty = 1) => this.difficulty = difficulty;


        /// <summary>
        ///
        /// </summary>
        /// <param name="minutes"></param>
        public virtual void SetMinutes(float minutes) => this.minutes = minutes;

        /// <summary>
        ///
        /// </summary>
        /// <param name="seconds"></param>
        public virtual void SetSeconds(float seconds) => this.seconds = seconds;
    }
}