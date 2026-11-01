//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
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
    public class IndicatorTutorialComponent : TutorialComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform indicatorParent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject indicatorPrefab = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite activeIndicator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite inactiveIndicator = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool noIndicatorsOnSinglePage = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform leftArrow = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform rightArrow = null;


        /// <summary>
        ///
        /// </summary>
        protected readonly List<Image> indicators = new();


        /// <inheritdoc/>
        public override void Initialize(TutorialAsset asset)
        {
            if (InitializationCheckFailed(asset))
                return;

            if (indicatorPrefab != null)
                InitializeIndicator(asset.GetPageCount());

            if (rightArrow != null)
                rightArrow.SetAsLastSibling();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected virtual bool InitializationCheckFailed(TutorialAsset asset)
        {
            if (rightArrow != null)
                rightArrow.gameObject.SetActive(false);

            if (leftArrow != null)
                leftArrow.gameObject.SetActive(false);

            if (asset == null || (noIndicatorsOnSinglePage && asset.GetPageCount() == 1))
                return true;

            if (rightArrow != null)
                rightArrow.gameObject.SetActive(true);

            if (leftArrow != null)
                leftArrow.gameObject.SetActive(true);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageCount"></param>
        protected virtual void InitializeIndicator(int pageCount)
        {
            for (int i = 0; i < pageCount; i++)
            {
                GameObject gameObject = Instantiate(indicatorPrefab);

                gameObject.transform.SetParent(indicatorParent, false);

                indicators.Add(gameObject.GetComponent<Image>());

                if (i == 0)
                    indicators[i].sprite = activeIndicator;

                indicators[i].transform.SetAsLastSibling();
            }
        }


        /// <inheritdoc/>
        public override IEnumerator ChangePageRoutine(int newIndex)
        {
            if (indicators.Count == 0)
                yield break;

            indicators[currentIndex].sprite = inactiveIndicator;

            indicators[newIndex].sprite = activeIndicator;

            currentIndex = newIndex;

            yield break;
        }


        /// <inheritdoc/>
        public override void PreResolve() { }


        /// <inheritdoc/>
        public override void Resolve()
        {
            foreach (Image image in indicators)
                Destroy(image.gameObject);

            indicators.Clear();
        }
    }
}