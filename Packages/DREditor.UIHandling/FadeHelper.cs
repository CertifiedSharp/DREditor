//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public static class FadeHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static Tweener DOFadeAlpha(this Component component, float endValue, float duration)
        {
            if (component == null) 
                return null;

            if (component is CanvasGroup cg)
                return cg.DOFade(endValue, duration);

            var colorProperty = component.GetType().GetProperty("color");

            if (colorProperty != null && colorProperty.PropertyType == typeof(Color))
            {
                Color startColor = (Color)colorProperty.GetValue(component);

                Tweener tweener = DOTween.To(
                    () => startColor,
                    color =>
                    {
                        startColor = color;
                        colorProperty.SetValue(component, color);
                    },
                    new Color(startColor.r, startColor.g, startColor.b, endValue),
                    duration);

                return tweener;
            }

            return null;
        }
    }
}
#endif