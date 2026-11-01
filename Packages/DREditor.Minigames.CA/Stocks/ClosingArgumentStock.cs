// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Stocks
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ClosingArgumentStockBase) + ", " + nameof(ILockPreview) +
        "\n\n")]
    public class ClosingArgumentStock : ClosingArgumentStockBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer spriteRenderer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteMask spriteMask = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer overlaySpriteRenderer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite unlockSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite lockSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite completeSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject hoverObject = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject clickedObject = null;


        /// <inheritdoc/>
        public override void SetStockVisual(object obj)
        {
            if (obj is Sprite sprite && spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;

                spriteRenderer.sortingOrder = transform.GetSiblingIndex();

                spriteMask.frontSortingOrder = transform.GetSiblingIndex();

                if (spriteMask.frontSortingOrder == 0)
                    spriteMask.backSortingOrder = -1;
            }
        }

        /// <inheritdoc/>
        public override object GetStockVisual() => spriteRenderer.sprite;

        /// <inheritdoc/>
        public override void UpdateLockState()
        {
            base.UpdateLockState();

            Sprite lockStateSprite = (IsUnlocked || ToBeUnlocked) ? unlockSprite : lockSprite;

            if (Solved)
                lockStateSprite = completeSprite;

            overlaySpriteRenderer.sprite = lockStateSprite;
        }

        /// <inheritdoc/>
        public override void SetHover(bool isHovering) => SetObjectActive(hoverObject, isHovering);

        /// <inheritdoc/>
        public override void SetClicked(bool isClicked) => SetObjectActive(clickedObject, isClicked);

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="condition"></param>
        protected virtual void SetObjectActive(GameObject go, bool condition)
        {
            if (go != null)
                go.SetActive(condition);
        }

        /// <inheritdoc/>
        public override void CAReset()
        {
            base.CAReset();

            SetHover(false);
        }
    }
}