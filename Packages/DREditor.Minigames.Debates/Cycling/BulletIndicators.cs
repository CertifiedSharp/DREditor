//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(BulletIndicatorsBase) + ", " + nameof(IBulletSendable) +
        "\n\n")]
    public class BulletIndicators : BulletIndicatorsBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite indicatorOn;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite indicatorOff;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Image> indicators = new();


        /// <inheritdoc/>
        protected override void InitializeIndicators()
        {
            if (indicators.Count > 0)
            {
                TurnOffIndicators();

                indicators[0].sprite = indicatorOn;

                for (int i = 0; i < truthBullets.Count && i < indicators.Count; i++)
                    indicators[i].enabled = true;
            }
        }

        
        /// <inheritdoc/>
        public override void TurnOffIndicators()
        {
            foreach (Image indicator in indicators)
                indicator.enabled = false;
        }

        /// <inheritdoc/>
        public override void SetIndicator(int index, bool on)
        {
            if (indicators.Count != 0)
                indicators[index].sprite = on ? indicatorOn : indicatorOff;
        }
    }
}