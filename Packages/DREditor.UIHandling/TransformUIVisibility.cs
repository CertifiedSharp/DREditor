//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TransformUIVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform target = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("showPosition")]
        [SerializeField] protected Vector3 showValue;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float showDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("hidePosition")]
        [SerializeField] protected Vector3 hideValue;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float hideDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool killOnPlayed = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool ignoreTimeScale = false;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease easeCurve = Ease.Linear;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int loops = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float delay = 0f;

#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected Tween activeTween;
#endif

        /// <summary>
        ///
        /// </summary>
        private const int InfiniteLoops = -1;


        protected virtual void Awake() => target = transform;

        /// <inheritdoc/>
        protected override void ShowUI() => PlayTween(showValue, showDuration);

        /// <inheritdoc/>
        protected override void HideUI() => PlayTween(hideValue, hideDuration);


        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        protected virtual void PlayTween(Vector3 to, float duration)
        {
#if DOTWEEN
            KillOnPlayed();

            AssignActiveTween(to, duration);

            ApplyTweenSettings();

            PlayActiveTween();
#endif
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected virtual void KillOnPlayed()
        {
            if (killOnPlayed)
                activeTween?.Kill();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        protected virtual void AssignActiveTween(Vector3 to, float duration) => activeTween = CreateTween(to, duration);

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        protected abstract Tween CreateTween(Vector3 to, float duration);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Tween ApplyTweenSettings() =>
            activeTween.SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops)
                .SetDelay(delay);

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlayActiveTween() => activeTween.Play();
#endif

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete() => new WaitUntil(() => ChangeConsideredComplete());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
#if DOTWEEN
        protected virtual bool ChangeConsideredComplete() => !DOTween.IsTweening(target) || loops == InfiniteLoops;
#else
        protected virtual bool ChangeConsideredComplete() => loops == InfiniteLoops;
#endif
    }
}