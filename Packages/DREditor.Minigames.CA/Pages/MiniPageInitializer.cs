// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PageInitializerBase) + ", " + nameof(IPageInitializer) + ", " + nameof(IClosingArgumentResetable) +
        "\n\n")]
    public class MiniPageInitializer : PageInitializerBase, IPageInitializer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float xMultiplier = 2.8f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 localScale = new(0.58f, 0.58f, 1);

        /// <summary>
        ///
        /// </summary>
        [Space(10)]

        [Tooltip("")]
        [SerializeField] protected int miniPanelSpriteSortOrder = 5;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int correctChildIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int correctSortOrder = 6;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int mysteryChildIndex = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int mysterySortOrder = 7;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int miniPanelStartingIndex = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int miniPanelSymbolIndex = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int miniPanelSymbolSortOrder = 8;


        /// <inheritdoc/>
        public virtual void OnPageSpawned(GameObject spawnedPage, int totalPages)
        {
            // Create copy of the page for the pages bar at the bottom of the screen
            GameObject miniPage =
                Instantiate(spawnedPage, pages.transform.position, Quaternion.Euler(0, 0, 0), pages.transform);

            miniPage.transform.position =
                new Vector3(
                    miniPage.transform.position.x + totalPages * xMultiplier,
                    miniPage.transform.position.y,
                    miniPage.transform.position.z);

            miniPage.transform.localScale = localScale;

            UpdateSpritesSortOrder(miniPage);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="miniPage"></param>
        protected virtual void UpdateSpritesSortOrder(GameObject miniPage)
        {
            try
            {
                if (miniPage.TryGetComponent(out SpriteRenderer miniPanelSprite))
                    miniPanelSprite.sortingOrder = miniPanelSpriteSortOrder;

                miniPage.transform
                    .GetChild(correctChildIndex)
                    .GetComponent<SpriteRenderer>()
                    .sortingOrder = correctSortOrder;

                miniPage.transform
                    .GetChild(mysteryChildIndex)
                    .GetComponent<SpriteRenderer>()
                    .sortingOrder = mysterySortOrder;

                for (int i = miniPanelStartingIndex; i < miniPage.transform.childCount; i++)
                    miniPage.transform
                        .GetChild(i)
                        .GetChild(miniPanelSymbolIndex)
                        .GetComponent<SpriteRenderer>()
                        .sortingOrder = miniPanelSymbolSortOrder;
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogWarning("ALERT: There was a problem updating Mini Pages sprite sort ordering!");

                UnityEngine.Debug.LogException(exception);
            }
        }


        /// <inheritdoc/>
        public virtual void SetUpPageDisplay() => UpdateRotation();
    }
}