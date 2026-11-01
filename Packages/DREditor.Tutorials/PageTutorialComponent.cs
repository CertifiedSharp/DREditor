//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TutorialComponent) +
        "\n\n")]
    public class PageTutorialComponent : TutorialComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image page = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float showVisibility = 1;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float hideVisibility = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float totalFadeTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float initializeDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveDuration = 1;


        /// <summary>
        ///
        /// </summary>
        protected List<Sprite> pages = new();


        /// <inheritdoc/>
        public override void Initialize(TutorialAsset asset)
        {
            foreach (var page in asset.pages)
                pages.Add(page.pageSprite);

            if (pages.Count > 0)
                page.sprite = pages[0];
#if DOTWEEN
            page.DOFade(showVisibility, initializeDuration).SetUpdate(true);
#else
            page.color = new(page.color.r, page.color.g, page.color.b, showVisibility);
#endif
        }

        /// <inheritdoc/>
        public override IEnumerator ChangePageRoutine(int newIndex)
        {
            float changeDuration = totalFadeTime / 2;
#if DOTWEEN
            page.DOFade(hideVisibility, changeDuration)
                .SetUpdate(true);
#else
            page.color = new(page.color.r, page.color.g, page.color.b, hideVisibility);
#endif
            yield return new WaitForSecondsRealtime(changeDuration);

            page.sprite = pages[newIndex];
#if DOTWEEN
            page.DOFade(showVisibility, changeDuration)
                .SetUpdate(true);
#else
            page.color = new(page.color.r, page.color.g, page.color.b, showVisibility);
#endif
            yield return new WaitForSecondsRealtime(changeDuration);
        }

        /// <inheritdoc/>
        public override void PreResolve()
        {
#if DOTWEEN
            page.DOFade(hideVisibility, resolveDuration).SetUpdate(true);
#else
            page.color = new(page.color.r, page.color.g, page.color.b, hideVisibility);
#endif
        }

        /// <inheritdoc/>
        public override void Resolve() => pages.Clear();
    }
}