//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public class LayeredFocusCarousel : CarouselSelectTweening
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected RectTransform carouselParent;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform focusedParent;


        /// <inheritdoc/>
        protected override bool IsComponentFocused(CarouselComponent component)
        {
            bool result = base.IsComponentFocused(component);

            component.transform.SetParent(result ? focusedParent : carouselParent);

            return result;
        }
    }
}