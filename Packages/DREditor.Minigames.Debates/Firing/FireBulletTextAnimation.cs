//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [ClassHeader(nameof(FireTruthAnimationBase) +
        "\n\n")]
    public class FireBulletTextAnimation : FireBulletTextAnimationBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float duration = 0.73f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float durationOffset = 0.2f;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease ease = Ease.Unset;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AnimationCurve curve = new(new Keyframe[]
        {
            new (0.00470733642578125f, 0.002349853515625f, 3.6899938583374025f,
                3.6899938583374025f, 0.3333333432674408f, 0.24263609945774079f),

            new (0.20443809032440186f, 0.49529412388801577f, -0.07345954328775406f,
                -0.07345954328775406f, 0.25743964314460757f, 0.10068052262067795f),

            new (0.4991734027862549f, 0.5783262848854065f, 0.5967904329299927f,
                0.5967904329299927f, 0.19783519208431245f, 0.13953030109405518f),

            new (1.0f, 1.0f, 0.013498243875801564f,
                0.013498243875801564f, 0.1620485782623291f, 0.3333333432674408f)
        });

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startXPivot = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endXPivot = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float firePositionZOffset = 0.005F;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fireScale = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float rectXPivotDivisor = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float positionZDurationDivisor = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fireAnimationDurationOffsetDivisor = 2;


        /// <summary>
        ///
        /// </summary>
        protected Vector3 resetPoint;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 resetScale;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            InitializeResetState();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeResetState()
        {
            resetPoint = transform.position;

            resetScale = transform.localScale;
        }


        /// <inheritdoc/>
        public override IEnumerator FireAnimation()
        {
            SetResetState();

            SetSizeDelta();

            yield return base.FireAnimation();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetResetState()
        {
            transform.position = resetPoint;

            transform.localScale = resetScale;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetSizeDelta()
        {
            rect.pivot = new(startXPivot, rect.pivot.y);

            Vector2 newSizeDelta = new(bulletText.renderedWidth, rect.sizeDelta.y);

            rect.sizeDelta = newSizeDelta;

            foreach (var cutOutText in cutOutTexts)
                cutOutText.sizeDelta = newSizeDelta;
        }


        /// <inheritdoc/>
        protected override void AnimateFiredBullet()
        {
#if DOTWEEN
            AnimatePosition();

            AnimateScale();

            AnimatePivotX();

            AnimateZPosition();
#endif
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimatePosition()
        {
            var move = transform.DOMove(firePosition, duration)
                .SetUpdate(true);

            ApplyEase(move);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateScale()
        {
            var scale = transform.DOScale(fireScale, duration)
                .SetUpdate(true);

            ApplyEase(scale);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tween"></param>
        protected virtual void ApplyEase(Tween tween)
        {
            if (ease == Ease.Unset)
                tween.SetEase(curve);
            else
                tween.SetEase(ease);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimatePivotX()
        {
            rect.DOPivotX(endXPivot, duration / rectXPivotDivisor * endXPivot)
                .SetUpdate(true)
                .SetDelay(duration - durationOffset);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AnimateZPosition()
        {
            transform.DOMoveZ(firePosition.z + firePositionZOffset, duration / positionZDurationDivisor)
                .SetUpdate(true)
                .SetDelay(duration - durationOffset);
        }
#endif

        /// <inheritdoc/>
        protected override float CalculateFireAnimationDuration() => duration - (durationOffset / fireAnimationDurationOffsetDivisor);

        /// <inheritdoc/>
        public override float CalculateRemainingDuration() => durationOffset * fireAnimationDurationOffsetDivisor;
    }
}