//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SpawnedCycleBulletsClearer) + ", " + nameof(IBulletSendable) +
        "\n\n")]
    public class BulletCycleUISpawner : SpawnedCycleBulletsClearer, IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform cycleBulletParent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject cycleBulletPrefab;


        [Header("")]
        [SerializeField] protected UnityEvent<Transform> onCycleBulletSpawned;


        [Header("")]
        [SerializeField] protected UnityEvent<List<GameObject>> onSpawningCompleted;


        /// <summary>
        ///
        /// </summary>
        protected Canvas canvas = null;


        /// <summary>
        ///
        /// </summary>
        protected override void OnBulletsInitialized() => StartCoroutine(SpawnCycleBullets());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SpawnCycleBullets()
        {
            OnPreSpawningBullets();

            yield return SpawnBulletsRoutine();

            OnPostSpawningBullets();

            OnSpawningCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPreSpawningBullets()
        {
            if (ReportIf(TryGetComponent(out canvas),
                $"REPORT: {nameof(BulletCycleUI)} is enabling it's canvas to ensure proper spawned bullet positions."))
                canvas.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool ReportIf(bool condition, string message)
        {
            if (condition)
                Debug.Log(message);

            return condition;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SpawnBulletsRoutine()
        {
            for (int i = 0; i < TruthBullets.Count; i++)
                yield return SpawnBulletRoutine();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPostSpawningBullets()
        {
            if (canvas != null)
                canvas.enabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SpawnBulletRoutine()
        {
            var spawn = Instantiate(cycleBulletPrefab, cycleBulletParent);

            SpawnedBullets?.Add(spawn);

            onCycleBulletSpawned?.Invoke(spawn.transform);

            yield return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSpawningCompleted() => onSpawningCompleted?.Invoke(SpawnedBullets);
    }
}