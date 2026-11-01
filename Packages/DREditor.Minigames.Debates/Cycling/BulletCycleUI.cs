//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates.Mechanics;
using DREditor.UIHandling;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SpawnedCycleBulletsClearer) + ", " + nameof(IBulletCycleUI) + ", " +
        nameof(IBulletSendable) + ", " + nameof(ICycleListener) +
        "\n\n")]
    public class BulletCycleUI : SpawnedCycleBulletsClearer, IBulletCycleUI, ICycleListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage bulletImage;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent cycleUIVisibility;


        /// <summary>
        ///
        /// </summary>
        protected bool isUp;

        /// <summary>
        ///
        /// </summary>
        protected int bulletIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected List<CycleUIBulletBase> uiBullets = new();


        /// <inheritdoc/>
        protected override void OnBulletsInitialized() => uiBullets.Clear();


        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnedBullets"></param>
        /// <remarks></remarks>
        public virtual void OnSpawningCompleted(List<GameObject> spawnedBullets)
        {
            InitializeSpawnedBullets(spawnedBullets);

            ValidateSpawnedBullets();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnedBullets"></param>
        protected virtual void InitializeSpawnedBullets(List<GameObject> spawnedBullets)
        {
            SpawnedBullets.Clear();

            SpawnedBullets.AddRange(spawnedBullets);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ValidateSpawnedBullets()
        {
            if (SpawnedBulletsAreValid())
                OnValidBulletsSpawned();
            else
                OnInvalidBulletsSpawned();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SpawnedBulletsAreValid()
        {
            foreach (var spawn in SpawnedBullets)
                if (spawn.GetComponent<CycleUIBulletBase>() is CycleUIBulletBase cycleBullet)
                    uiBullets.Add(cycleBullet);
                else
                    return false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnValidBulletsSpawned()
        {
            SetUIBulletTexts();

            UpdateActiveBullet();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInvalidBulletsSpawned()
        {
            ResolveCycleBullets();

            UnityEngine.Debug.LogWarning("ALERT: The Cycle Bullet Prefab does not contain the required " +
                $"{nameof(CycleUIBulletBase)} script." +
                $"\nPROPOSAL: Ensure that the filled out prefab on your bullet spawner contains the required script.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetUIBulletTexts()
        {
            for (int i = 0; i < TruthBullets.Count && i < uiBullets.Count; i++)
                uiBullets[i].UpdateBulletText(TruthBullets[i].DisplayName);
        }


        /// <inheritdoc/>
        public virtual void StartCycle() => cycleUIVisibility.Show();

        /// <inheritdoc/>
        public virtual void EndCycle() => cycleUIVisibility.Hide();

        /// <inheritdoc/>
        public virtual void OnBulletCycled(bool isUp)
        {
            if (TruthBullets.Count == 1)
                return;

            this.isUp = isUp;

            SwapActiveBullet();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SwapActiveBullet()
        {
            DisplayBulletActive(false);

            UpdateBulletIndex();

            UpdateActiveBullet();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateActiveBullet()
        {
            DisplayBulletActive(true);

            UpdateBulletImage();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isActive"></param>
        protected virtual void DisplayBulletActive(bool isActive)
        {
            if (bulletIndex < uiBullets.Count)
                uiBullets[bulletIndex].DisplayBulletActive(isActive);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateBulletIndex()
        {
            int appliedOffset = isUp ? -1 : 1;

            bulletIndex += appliedOffset;

            int outOfBoundsValue = isUp ? -1 : TruthBullets.Count;

            if (bulletIndex == outOfBoundsValue)
                bulletIndex = isUp ? TruthBullets.Count - 1 : 0;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateBulletImage()
        {
            if (bulletImage != null && bulletIndex < TruthBullets.Count && TruthBullets[bulletIndex].Image != null)
                bulletImage.texture = TruthBullets[bulletIndex].Image.texture;
        }
    }
}