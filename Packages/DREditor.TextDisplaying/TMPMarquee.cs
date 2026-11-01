//OG Author: KHeartz. My Magic Man
//Edited Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TMPMarqueeBase) +
        "\n\n")]
    public class TMPMarquee : TMPMarqueeBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float scrollDuration = 5;


        /// <summary>
        ///
        /// </summary>
        protected float currentWidth;

        /// <summary>
        ///
        /// </summary>
        protected float startX;

        /// <summary>
        ///
        /// </summary>
        protected float elapsedSeconds;


        /// <inheritdoc/>
        protected override void Initialize()
        {
            ResetPosition();

            ForceUpdateWidth();

            ResetCalculatedVariables();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetPosition() => marqueeTransform.localPosition = new(parentMask.rect.xMax, 0, 0);

        /// <summary>
        ///
        /// </summary>
        protected virtual void ForceUpdateWidth()
        {
            tmp.ForceMeshUpdate();

            currentWidth = tmp.rectTransform.rect.width;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetCalculatedVariables()
        {
            startX = marqueeTransform.localPosition.x;

            elapsedSeconds = 0f;
        }


        /// <inheritdoc/>
        protected override IEnumerator MarqueeRoutine()
        {
            while (true)
            {
                Initialize();

                while (MarqueeUpdated())
                    yield return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual bool MarqueeUpdated()
        {
            bool result = ShouldBeScrolling();

            if (result)
                UpdateScroll();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldBeScrolling() => elapsedSeconds < scrollDuration;


        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateScroll()
        {
            CheckWidthChanged();

            TickElapsed();

            UpdatePosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckWidthChanged()
        {
            if (WidthHasChanged())
            {
                ForceUpdateWidth();

                CalculateElapsedSeconds();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool WidthHasChanged() => Mathf.Abs(tmp.rectTransform.rect.width - currentWidth) > 0.1f;

        /// <summary>
        ///
        /// </summary>
        protected virtual void CalculateElapsedSeconds()
        {
            float currentX = marqueeTransform.localPosition.x;

            float totalDistance = startX - ScrollEndXPosition();

            float remainingDistance = currentX - ScrollEndXPosition();

            float progress = 1f - (remainingDistance / totalDistance);

            elapsedSeconds = progress * scrollDuration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float ScrollEndXPosition() => parentMask.rect.xMin - tmp.rectTransform.rect.width;

        /// <summary>
        ///
        /// </summary>
        protected virtual void TickElapsed() => elapsedSeconds += Time.deltaTime;

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdatePosition()
        {
            float interpolationPercentage = Mathf.Clamp01(elapsedSeconds / scrollDuration);

            float calculatedX = Mathf.Lerp(startX, ScrollEndXPosition(), interpolationPercentage);

            marqueeTransform.localPosition = new Vector3(calculatedX, 0, 0);
        }
    }
}