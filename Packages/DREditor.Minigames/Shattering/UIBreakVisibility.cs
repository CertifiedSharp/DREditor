//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev also helped out a lot
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Shattering
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class UIBreakVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Canvas screenCanvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage blackBreakImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage breakImage = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool turnOffCanvasAtEnd = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float shatterDuration = 5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float enableDuration = 0f;

        /// <summary>
        ///
        /// </summary>
        [Range(0f, 1f)]
        [Tooltip("")]
        [SerializeField] protected float enableVisibility = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float enableScale = 1f;

        /// <summary>
        ///
        /// </summary>
        [Range(0f, 1f)]
        [Tooltip("")]
        [SerializeField] protected float endVisibility = 0f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float showDuration = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float showScale = 2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float scaleDelay = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float breakDelay = 1.5f;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease easeCurve = Ease.InFlash;
#endif

        protected virtual void Start()
        {
            if (!screenCanvas)
                screenCanvas = gameObject.GetComponentInParent<Canvas>();
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
            if (turnOffCanvasAtEnd)
                screenCanvas.enabled = false;
        }

        /// <inheritdoc/>
        protected override void ShowUI()
        {
            if (screenCanvas != null)
                screenCanvas.enabled = true;
#if DOTWEEN
            breakImage.transform.DOScale(enableScale, enableDuration);

            breakImage.DOFade(enableVisibility, enableDuration);

            blackBreakImage.DOFade(enableVisibility, enableDuration);
#endif
            StartCoroutine(BufferShow());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferShow()
        {
            yield return null;

            blackBreakImage.enabled = true;

            breakImage.enabled = true;
#if DOTWEEN
            breakImage.transform.DOScale(showScale, showDuration)
                .SetDelay(scaleDelay)
                .SetEase(easeCurve);

            breakImage.DOFade(endVisibility, showDuration)
                .SetDelay(breakDelay)
                .SetEase(easeCurve);

            blackBreakImage.DOFade(endVisibility, showDuration)
                .SetDelay(shatterDuration)
                .OnComplete(Hide);
#endif
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
#if DOTWEEN
            yield return new WaitUntil(() => !DOTween.IsTweening(blackBreakImage));
#else
            yield return null;
#endif
        }
    }
}