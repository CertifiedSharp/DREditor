//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.LineReading;
using DREditor.Services;
using System.Collections;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TrialStatsBase : ServiceHolder<TrialStatsBase>, ITweenWaiter
    {
        /// <summary>
        ///
        /// </summary>
        public abstract IEnumerator GameOverResetProcess();

        /// <summary>
        ///
        /// </summary>
        public abstract void ShowTrialStats();

        /// <summary>
        ///
        /// </summary>
        public abstract void HideTrialStats();

        /// <summary>
        ///
        /// </summary>
        /// <param name="damage"></param>
        public abstract void TakeDamage(float damage = 0);

        /// <inheritdoc/>
        public abstract IEnumerator WaitForTweenToFinish();
    }
}