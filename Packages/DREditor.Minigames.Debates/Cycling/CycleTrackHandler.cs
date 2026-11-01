//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Minigames.Debates.Mechanics;
using DREditor.Investigations;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) + ", " +
        nameof(ICycleListener) + ", " + nameof(IBulletSendable) + ", " + nameof(ICycleTracker) +
        "\n\n")]
    public class CycleTrackHandler : UIVisibilityComponent, ICycleListener, IBulletSendable, ICycleTracker
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<CyclePosition> positions = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int firstVisible = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int notVisible = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int maxVisible = 5;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float toFirstDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float toPositionDuration = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float cycleDuration = 0.2f;


        /// <summary>
        ///
        /// </summary>
        protected int bulletCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected bool moveForwardOnFinal = false;

        /// <summary>
        ///
        /// </summary>
        protected TrackPathCalculator pathCalculator;

        /// <summary>
        ///
        /// </summary>
        protected readonly Dictionary<Transform, CyclePosition> pairs = new();


        protected virtual void Awake() => pathCalculator = new TrackPathCalculator(positions, firstVisible, notVisible);


        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        /// <remarks></remarks>
        public virtual void OnTransformSpawned(Transform transform)
        {
            MoveAll(true, true);

            Register(transform);

            MoveToFirstPosition(transform, true);

            Show();

            if (MoveForwardOnFinal())
                MoveAll(true, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool MoveForwardOnFinal() => moveForwardOnFinal && pairs.Count == bulletCount;


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets)
        {
            bulletCount = bullets.Count;

            pathCalculator.InitializeVisibility(bulletCount, maxVisible);

            if (pairs.Count > 0)
                moveForwardOnFinal = true;
        }


        /// <inheritdoc/>
        public virtual void OnBulletCycled(bool isUp) => MoveAll(!isUp);


        /// <inheritdoc/>
        public virtual void MoveAll(bool forward, bool immediate = false)
        {
            foreach (Transform transform in pairs.Keys.ToList())
                MoveTransform(transform, forward, immediate ? 0f : cycleDuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="forward"></param>
        /// <param name="duration"></param>
        protected virtual void MoveTransform(Transform transform, bool forward, float duration)
        {
#if DOTWEEN
            transform.DOKill();
#endif
            Register(transform);

            CyclePosition currentPos = pairs[transform];

            var (path, nextPosition) = pathCalculator.CalculateCyclePath(currentPos, forward, pairs.Count);

            if (duration <= 0f)
                transform.position = path.Last();
            else
#if DOTWEEN
                transform.DOPath(path, duration);
#else
                transform.position = pairs[transform].GetSubPositions()[0];
#endif

            pairs[transform] = nextPosition;
        }


        /// <inheritdoc/>
        public virtual float MoveToFirstPosition(Transform transform, bool immediate = false)
        {
            if (positions.Count == 0 || positions[0] == null)
                return 0;

            float duration = immediate ? 0f : toFirstDuration;

            Vector3 targetPos = positions[0].transform.position;
#if DOTWEEN
            transform.DOKill();
#endif
            if (immediate)
            {
                transform.position = targetPos;

                Register(transform);
            }
            else
#if DOTWEEN
                transform.DOMove(targetPos, duration)
                         .OnComplete(() => Register(transform));
#else
                transform.Translate(targetPos);
#endif
            return duration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        protected virtual void Register(Transform transform)
        {
            if (!pairs.ContainsKey(transform) && positions.Count > 0 && positions[0] != null)
                pairs.Add(transform, positions[0]);
        }

        /// <inheritdoc/>
        public virtual void ClearTrack() => pairs.Clear();


        /// <inheritdoc/>
        protected override void ShowUI()
        {
            foreach (var pair in pairs)
            {
#if DOTWEEN
                pair.Key.DOKill();

                pair.Key.DOMove(pair.Value.transform.position, toPositionDuration);
#endif
            }
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
            foreach (var pair in pairs)
            {
#if DOTWEEN
                pair.Key.DOKill();
#endif
                if (pathCalculator.InVisibleRange(pair.Value))
                    continue;

                MoveToFirstPosition(pair.Key);
            }
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
#if DOTWEEN
            if (pairs.Keys.Count > 0)
                yield return new WaitUntil(() => !DOTween.IsTweening(pairs.Keys.First()));
#endif
            yield return null;
        }


        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < positions.Count; i++)
            {
                int endPositionIndex = i != positions.Count - 1 ? i + 1 : 0;

                Gizmos.DrawLine(positions[i].GetSubPositions()[^1], positions[endPositionIndex].transform.position);
            }
        }
    }
}