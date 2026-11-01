// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.CA.Stocks
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ActiveStockPanelStateable) + ", " + nameof(ActiveStockPanelBase) + ", " +
        nameof(IClosingArgumentListener) + ", " + nameof(ILockPreview) +
        "\n\n")]
    public class ActiveStockPanel : ActiveStockPanelStateable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent panelShootAnimation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent unlockUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer borderSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text activePanelText;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer activeStockRenderer;

        
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Sprite inactivePanelSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite activePanelSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite lockedPanelSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite unlockPanelSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite solvedPanelSprite = null;


        /// <inheritdoc/>
        protected override void OnStockUnsolved()
        {
            borderSprite.sprite = inactivePanelSprite;

            ApplyStockToPanel();
        }

        /// <inheritdoc/>
        protected override void OnStockSolved()
        {
            borderSprite.sprite = solvedPanelSprite;

            ApplyStockToPanel();
        }

        /// <inheritdoc/>
        protected override void OnStockUnlock()
        {
            base.OnStockUnlock();

            borderSprite.sprite = unlockPanelSprite;

            StartCoroutine(OnStockUnlockRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator OnStockUnlockRoutine()
        {
            DisableInput();

            activePanelText.text = "";

            if (unlockUI != null)
                yield return unlockUI.ShowTillComplete();

            currentStock.UpdateLockState();

            ApplyStockToPanel();

            borderSprite.sprite = inactivePanelSprite;

            EnableInput();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyStockToPanel()
        {
            activePanelText.text = currentStock.flavorText;

            activeStockRenderer.sprite = (Sprite)currentStock.GetStockVisual();

            activeStockRenderer.transform.localPosition = currentStock.activePanelPos;

            activeStockRenderer.transform.localScale = currentStock.activePanelScale;
        }


        /// <inheritdoc/>
        protected override void OnStockLocked()
        {
            borderSprite.sprite = lockedPanelSprite;

            if (currentStock.remainingLocks == 1)
                activePanelText.text = "Solve another panel to unlock.";
            else
                activePanelText.text = "Solve another " + currentStock.remainingLocks.ToString() + " panels to unlock.";
        }


        /// <inheritdoc/>
        protected override IEnumerator OnStockShot()
        {
            borderSprite.sprite = activePanelSprite;

            yield return panelShootAnimation.ShowTillComplete();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="hoveredObject"></param>
        public override void OnStockCorrect(Transform hoveredObject)
        {
            borderSprite.sprite = solvedPanelSprite;

            base.OnStockCorrect(hoveredObject);
        }
    }
}