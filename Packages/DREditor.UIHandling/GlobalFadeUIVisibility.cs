//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class GlobalFadeUIVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.5f;


        /// <inheritdoc/>
        protected override void ShowUI() => ServiceLocator.GetService<GlobalFade>().FadeTo(fadeToDuration);

        /// <inheritdoc/>
        protected override void HideUI() => ServiceLocator.GetService<GlobalFade>().FadeOut(fadeOutDuration);

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            while (ServiceLocator.GetService<GlobalFade>().isTransitioning)
                yield return null;

            yield break;
        }
    }
}