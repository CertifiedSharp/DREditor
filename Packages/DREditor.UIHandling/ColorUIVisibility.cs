//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) +
        "\n\n")]
    public class ColorUIVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Graphic> graphics = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Color showColor = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float colorShowDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color hideColor = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float colorHideDuration = 0.5f;


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

#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected List<Tween> activeTweens = new();
#endif

        /// <inheritdoc/>
        protected override void ShowUI()
        {
#if DOTWEEN
            graphics.ForEach(component =>
            RegisterTween(component.DOColor(showColor, colorShowDuration)
            .SetUpdate(ignoreTimeScale)
            .SetEase(easeCurve)
            .SetLoops(loops)
            .SetDelay(delay))
            );
#endif
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
#if DOTWEEN
            graphics.ForEach(component =>
            RegisterTween(component.DOColor(hideColor, colorHideDuration)
            .SetUpdate(ignoreTimeScale)
            .SetEase(easeCurve)
            .SetLoops(loops)
            .SetDelay(delay))
            );
#endif
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        /// <param name="tween"></param>
        protected virtual void RegisterTween(Tween tween)
        {
            tween.onComplete = () => { activeTweens.Remove(tween); };

            activeTweens.Add(tween);
        }
#endif
        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
#if DOTWEEN
            while (activeTweens.Count != 0)
#endif
                yield return null;
        }
    }
}