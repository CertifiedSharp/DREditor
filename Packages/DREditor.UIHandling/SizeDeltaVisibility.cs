//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class SizeDeltaVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<RectTransform> transformComponents = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Vector2 showSize = new(1602, 795);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float showDuration = 0.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector2 hideSize = new(1602, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float hideDuration = 0.25f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
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


        /// <inheritdoc/>
        protected override void ShowUI() => transformComponents.ForEach(component =>
#if DOTWEEN
        component.DOSizeDelta(showSize, showDuration)
        .SetUpdate(ignoreTimeScale)
        .SetEase(easeCurve)
        .SetLoops(loops)
        .SetDelay(delay)
        );
#else
        component.Translate(showSize));
#endif

        /// <inheritdoc/>
        protected override void HideUI() => transformComponents.ForEach(component =>
#if DOTWEEN
        component.DOSizeDelta(hideSize, hideDuration)
        .SetUpdate(ignoreTimeScale)
        .SetEase(easeCurve)
        .SetLoops(loops)
        .SetDelay(delay)
        );
#else
        component.Translate(hideSize));
#endif

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
#if DOTWEEN
            while (DOTween.IsTweening(transformComponents))
#endif
                yield return null;
        }
    }
}