//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Investigations;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) + ", " + nameof(IBulletSendable) +
        "\n\n")]
    public class IntroBulletCycleHandler : UIVisibilityComponent, IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject cycleBulletPrefab;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent track;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform spawnTransform;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool forcePrefabScaleToOne = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postIntroBufferDuration = 1;


        [Header("")]
        [SerializeField] protected UnityEvent OnLoadBullet;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<TruthBullet> truthBullets = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Transform> spawnedBullets = new();


        /// <summary>
        ///
        /// </summary>
        protected bool animating = false;

        /// <summary>
        ///
        /// </summary>
        protected bool debugging = false;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine activeIntroCoroutine;

        /// <summary>
        ///
        /// </summary>
        protected ICycleTracker cycleTracker;


        /// <summary>
        ///
        /// </summary>
        private const float FallbackSpawnMultiplier = 1.5f;

        /// <summary>
        ///
        /// </summary>
        private const float FallbackSpawnDivisor = 2;


        protected virtual void Awake()
        {
            if (track is ICycleTracker trackToCycle)
                cycleTracker = trackToCycle;
            else
                UnityEngine.Debug.LogWarning($"ANALYSIS: Field {nameof(track)} on {gameObject.name} " +
                    $"does not inherit expected type {nameof(ICycleTracker)} and therefore may not work as intended." +
                    $"\nPROPOSAL: Assign a component inheriting type {nameof(ICycleTracker)}.");
        }


        /// <inheritdoc/>
        protected override void ShowUI()
        {
            if (animating)
            {
                UnityEngine.Debug.LogWarning("ALERT: Already animating, ignoring Show() call.");

                return;
            }

            debugging = false;

            activeIntroCoroutine = StartCoroutine(AnimateIntro());
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
            if (animating && activeIntroCoroutine != null)
            {
                StopCoroutine(activeIntroCoroutine);

                activeIntroCoroutine = null;

                animating = false;
            }

            StartCoroutine(HideRoutine());
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            if (animating)
                yield return new WaitUntil(() => !animating);
            else
                yield return visibilityComponent.WaitForChangeToComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator AnimateIntro()
        {
            if (track == null)
            {
                UnityEngine.Debug.LogError("ALERT: Track handler not assigned, abandoning animation.");

                yield break;
            }

            animating = true;

            yield return ShowRoutine();

            yield return IntroCycle();

            yield return new WaitForSeconds(postIntroBufferDuration);

            yield return HideRoutine();

            animating = false;

            yield return null;

            if (!debugging)
                Destroy(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowRoutine()
        {
            if (visibilityComponent != null)
                yield return visibilityComponent.ShowTillComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator IntroCycle()
        {
            for (int i = 0; i < spawnedBullets.Count; i++)
            {
                float wait = (float)(cycleTracker?.MoveToFirstPosition(spawnedBullets[i].transform));

                OnLoadBullet?.Invoke();

                cycleTracker?.MoveAll(true);

                yield return new WaitForSeconds(wait);
            }

            cycleTracker?.MoveAll(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator HideRoutine()
        {
            if (visibilityComponent != null)
                yield return visibilityComponent.HideTillComplete();
        }


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets)
        {
            truthBullets = bullets;

            SpawnBullets(bullets);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bullets"></param>
        protected virtual void SpawnBullets(List<TruthBullet> bullets)
        {
            if (cycleBulletPrefab == null)
            {
                UnityEngine.Debug.LogError("ALERT: There is no " + nameof(cycleBulletPrefab) + " assigned!" +
                    "\nAbandoning spawning bullets.");

                return;
            }

            foreach (TruthBullet bullet in bullets)
            {
                var spawn = Instantiate(cycleBulletPrefab, GetSpawnPosition(), cycleBulletPrefab.transform.rotation);

                spawn.transform.SetParent(transform, true);

                if (forcePrefabScaleToOne)
                    spawn.transform.localScale = Vector3.one;

                if (spawn.GetComponentInChildren<TMP_Text>() is TMP_Text bulletText)
                    bulletText.SetText(bullet.DisplayName);

                spawnedBullets.Add(spawn.transform);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetSpawnPosition()
            => spawnTransform != null ?
            spawnTransform.localPosition : new(Screen.width * FallbackSpawnMultiplier, Screen.height / FallbackSpawnDivisor);


        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public void DebugTestAnimation()
        {
            if (animating)
            {
                UnityEngine.Debug.LogWarning("REPORT: You must wait for the already playing animation to finish.");

                return;
            }

            debugging = true;

            if (spawnedBullets.Count > 0)
                DestroySpawnedBullets();

            if (truthBullets.Count > 0)
                SpawnBullets(truthBullets);
            else
                UnityEngine.Debug.LogError("ALERT: This Intro is in debug mode yet no Truth Bullets have been assigned." +
                    "\nPROPOSAL: Assign Truth Bullets in the inspector so that this animation will display properly.");

            StartCoroutine(AnimateIntro());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroySpawnedBullets()
        {
            foreach (var bullet in spawnedBullets)
                Destroy(bullet.gameObject);

            spawnedBullets.Clear();

            cycleTracker?.ClearTrack();
        }
    }
}