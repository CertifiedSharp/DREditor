//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    public interface ITieBreaker
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="result"></param>
        public abstract IEnumerator TieBreakerProcess(Action<bool> result);
    }
}