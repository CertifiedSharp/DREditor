//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) +
        "\n\n")]
    public class SlidePositionVisibility : UIVisibilityComponent
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
        [SerializeField] bool localPosition = true;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] bool showLocalPosition = true;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] bool hideLocalPosition = true;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform showTarget;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float showDuration = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform hideTarget;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float hideDuration = 0.2f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform ogTarget;

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
        [SerializeField] protected Ease easeCurve = Ease.OutQuart;
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
        protected Vector3 ogPosition;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (ogTarget != null)
                ogPosition = localPosition ? ogTarget.localPosition : ogTarget.position;
            else
                ogPosition = localPosition ? target.localPosition : target.position;
        }

        /// <inheritdoc/>
        protected override void ShowUI()
        {
#if DOTWEEN
            Vector3 showPosition = showLocalPosition ? showTarget.localPosition : showTarget.position;

            if (showLocalPosition)
                target.DOLocalMove(showPosition, 0).SetUpdate(ignoreTimeScale);
            else
                target.DOMove(showPosition, 0).SetUpdate(ignoreTimeScale);

            if (localPosition)
                activeTween = target.DOLocalMove(ogPosition, showDuration)
                .SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops)
                .SetDelay(delay);
            else
                activeTween = target.DOMove(ogPosition, showDuration)
                .SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops)
                .SetDelay(delay);
#endif
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
#if DOTWEEN
            Vector3 hidePosition = hideLocalPosition ? hideTarget.localPosition : hideTarget.position;

            if (hideLocalPosition)
                activeTween = target.DOLocalMove(hidePosition, hideDuration)
                .SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops)
                .SetDelay(delay);
            else
                activeTween = target.DOMove(hidePosition, hideDuration)
                .SetUpdate(ignoreTimeScale)
                .SetEase(easeCurve)
                .SetLoops(loops)
                .SetDelay(delay);
#endif
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
#if DOTWEEN
            yield return new WaitUntil(() => !activeTween.IsPlaying());
#else
            yield return null;
#endif
        }
    }
}