//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(InfluenceGaugeBase) +
        "\n\n")]
    public class InfluenceGauge : InfluenceGaugeBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image influenceBar = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RectTransform influenceRect = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color influenceColor;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color drainColor;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float xPosDivideValue = 238;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float maxStamina = 10;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float staminaRatio = 2f;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected Vector3 influencePosition;


        /// <inheritdoc/>
        public override void DrainStaminaVisual()
        {
            influenceBar.color = drainColor;

            influencePosition = influenceRect.localPosition;

            influencePosition.x -= xPosDivideValue / maxStamina * Time.unscaledDeltaTime * staminaRatio;

            influenceRect.localPosition = influencePosition;
        }

        /// <inheritdoc/>
        public override bool StaminaVisualWrong() => influencePosition.x > 0;

        /// <inheritdoc/>
        public override void RegenStaminaVisual()
        {
            influenceBar.color = influenceColor;

            influencePosition = influenceRect.localPosition;

            influencePosition.x += xPosDivideValue / maxStamina * Time.unscaledDeltaTime * staminaRatio;

            influenceRect.localPosition = influencePosition;

            if (influencePosition.x > 0)
            {
                influencePosition.x = 0;

                influenceRect.localPosition = influencePosition;
            }
        }

        /// <inheritdoc/>
        public override void ResetStamina()
        {
            influenceBar.color = influenceColor;

            influencePosition.x = 0;

            influenceRect.localPosition = influencePosition;
        }
    }
}