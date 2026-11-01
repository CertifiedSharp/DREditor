//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HealthStatBase) + ", " + nameof(ColorHealthModifier) +
        "\n\n")]
    public class ColorHealthBar : ColorHealthModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image fillImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float regenHealthFillDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float colorDuration = 1;


        /// <inheritdoc/>
        public override void ResetHealth()
        {
#if DOTWEEN
            fillImage.DOColor(regenColor, 0);

            fillImage.DOColor(colorRange[0], regenHealthFillDuration)
                    .SetDelay(regenHealthFillDuration);
#endif
        }

        /// <inheritdoc/>
        protected override void ApplyColor() =>
#if DOTWEEN
        fillImage.DOColor(currentColor, colorDuration);
#else
        fillImage.color = currentColor;
#endif
    }
}