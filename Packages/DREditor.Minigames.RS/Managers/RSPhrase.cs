//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using System;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RSPhrase : Phrase, IPhraseHealth
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int initialHealth = 1;


        /// <summary>
        ///
        /// </summary>
        protected int health = 1;


        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();

            health = initialHealth;
        }

        /// <inheritdoc/>
        public virtual void SetInitialHealth(int initialHealth) => this.initialHealth = initialHealth;

        /// <inheritdoc/>
        public virtual int GetInitialHealth() => initialHealth;

        /// <inheritdoc/>
        public virtual int GetHealth() => health;

        /// <inheritdoc/>
        public virtual int TakeDamage(int damage)
        {
            health -= damage;
#if UNITY_EDITOR
            if (phraseText != "")
                UnityEngine.Debug.Log("REPORT: You hit the words: " + phraseText +
                    " Phrase Health is now: " + GetHealth());
            else
                UnityEngine.Debug.LogWarning("ALERT: You hit a Phrase Text with no words. This shouldn't be possible.");
#endif
            return health;
        }
    }
}