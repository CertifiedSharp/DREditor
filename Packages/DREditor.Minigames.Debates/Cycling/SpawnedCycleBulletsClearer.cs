//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SpawnedCycleBulletsClearer : MonoBehaviour, IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> TruthBullets { get; private set; }

        /// <summary>
        ///
        /// </summary>
        protected readonly List<GameObject> SpawnedBullets = new();


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets)
        {
            TruthBullets = bullets;

            ResolveCycleBullets();

            OnBulletsInitialized();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnBulletsInitialized();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveCycleBullets()
        {
            DestroyBullets();

            ClearBullets();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroyBullets()
        {
            if (SpawnedBullets != null)
                foreach (var bullet in SpawnedBullets)
                    Destroy(bullet);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearBullets() => SpawnedBullets?.Clear();


        protected virtual void OnDestroy() => ResolveCycleBullets();
    }
}