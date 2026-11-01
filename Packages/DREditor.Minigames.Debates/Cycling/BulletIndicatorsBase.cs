//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BulletIndicatorsBase : MonoBehaviour, IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<TruthBullet> truthBullets = new();


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets)
        {
            truthBullets = bullets;

            InitializeIndicators();
        }


        /// <summary>
        ///
        /// </summary>
        protected abstract void InitializeIndicators();


        /// <summary>
        ///
        /// </summary>
        public abstract void TurnOffIndicators();


        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="on"></param>
        public abstract void SetIndicator(int index, bool on);
    }
}