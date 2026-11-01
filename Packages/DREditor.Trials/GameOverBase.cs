//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GameOverBase : MonoBehaviour
    {
        protected virtual void OnEnable() => SubscribeToPlayerDeath();

        protected virtual void OnDisable() => UnsubscribeToPlayerDeath();

        /// <summary>
        ///
        /// </summary>
        protected abstract void SubscribeToPlayerDeath();

        /// <summary>
        ///
        /// </summary>
        protected abstract void UnsubscribeToPlayerDeath();

        /// <summary>
        ///
        /// </summary>
        protected abstract void GameIsOver();

        /// <summary>
        ///
        /// </summary>
        public abstract void GiveUp();

        /// <summary>
        ///
        /// </summary>
        public abstract void ContinueGame();
    }
}