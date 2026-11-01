//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [Serializable]
    public class MinigameBuilderBase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        public List<TimerDifficulty> times = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="times"></param>
        public virtual void SetDifficultyTimes(List<TimerDifficulty> times) => this.times = times;
    }
}