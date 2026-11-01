//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving.Handlers;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Stats
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ColorHealthModifier : HealthStatBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Color[] colorRange = new Color[3]
        {
            new Color32(32, 133, 47, 255), new Color32(255, 116, 1, 255), new(255, 0, 0, 255)
        };

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color regenColor = new Color32(144, 247, 198, 255);


        /// <summary>
        ///
        /// </summary>
        protected Color[] colorSteps;

        /// <summary>
        ///
        /// </summary>
        protected Color currentColor;

        /// <summary>
        ///
        /// </summary>
        protected HealthDataHandler<int> healthDataHandler = null;


        protected virtual void Start()
        {
            healthDataHandler = transform.root.GetComponentInChildren<HealthDataHandler<int>>();

            colorSteps = GenerateColorSteps(colorRange, healthDataHandler.GetMaxHealth());

            SetCurrentColor(colorSteps[0]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="anchorColors"></param>
        /// <param name="totalSteps"></param>
        protected virtual Color[] GenerateColorSteps(Color[] anchorColors, int totalSteps)
        {
            if (anchorColors == null || anchorColors.Length == 0)
                return new Color[totalSteps];

            if (anchorColors.Length == 1) 
                return Enumerable.Repeat(anchorColors[0], totalSteps).ToArray();

            if (totalSteps < anchorColors.Length) 
                totalSteps = anchorColors.Length;

            Color[] steps = new Color[totalSteps];

            int segmentsCount = anchorColors.Length - 1;

            float stepsPerSegment = (float)(totalSteps - 1) / segmentsCount;

            for (int i = 0; i < totalSteps; i++)
            {
                float globalPosition = i / stepsPerSegment;

                int currentAnchorIndex = Mathf.FloorToInt(globalPosition);

                if (currentAnchorIndex >= segmentsCount)
                {
                    steps[i] = anchorColors[^1];

                    continue;
                }

                int nextAnchorIndex = currentAnchorIndex + 1;

                float localT = globalPosition - currentAnchorIndex;

                steps[i] = Color.Lerp(anchorColors[currentAnchorIndex], anchorColors[nextAnchorIndex], localT);
            }

            return steps.Reverse().ToArray();
        }


        /// <inheritdoc/>
        public override void Refresh() => CalculateColor();

        /// <inheritdoc/>
        public override void TakeDamage(float damage) => CalculateColor();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CalculateColor()
        {
            UpdateColor();

            ApplyColor();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateColor()
        {
            if (healthDataHandler.GetHealth() - 1 >= 0 && healthDataHandler.GetHealth() - 1 < colorSteps.Length)
                SetCurrentColor(colorSteps[healthDataHandler.GetHealth() - 1]);
            else
                SetCurrentColor(colorRange[^1]);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void ApplyColor();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetCurrentColor(Color color) => currentColor = color;
    }
}