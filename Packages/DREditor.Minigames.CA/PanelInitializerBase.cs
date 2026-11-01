// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PanelInitializerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject panelPrefab = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject stockParent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int symbolChildIndex = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float positionMultiplier = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float positionDivider = 5;


        /// <summary>
        ///
        /// </summary>
        protected ArgumentQuestionPanel panelData;

        /// <summary>
        ///
        /// </summary>
        protected Transform page;

        /// <summary>
        ///
        /// </summary>
        protected GameObject qstnPanel;


        /// <summary>
        ///
        /// </summary>
        /// <param name="panelData"></param>
        /// <param name="page"></param>
        public virtual void InitializePanel(ArgumentQuestionPanel panelData, Transform page)
        {
            this.panelData = panelData;

            this.page = page;

            InitializeQuestionPanelVisual();

            InitializeSymbol();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeQuestionPanelVisual()
        {
            qstnPanel = Instantiate(panelPrefab, page.position, Quaternion.identity, page);

            qstnPanel.transform.position +=
                new Vector3(panelData.panelPos.x, panelData.panelPos.y, 0) *
                positionMultiplier / positionDivider;

            InitializePanelComponents();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePanelComponents()
        {
            if (qstnPanel.TryGetComponent(out SpriteRenderer spriteRenderer))
                spriteRenderer.sprite = panelData.panelShape;
            else
                UnityEngine.Debug.LogWarning("ALERT: Could not find a sprite renderer on the question panel!");

            qstnPanel.AddComponent<PolygonCollider2D>().isTrigger = true;

            if (qstnPanel.TryGetComponent(out SpriteMask spriteMask))
                spriteMask.sprite = panelData.panelShape;
            else
                UnityEngine.Debug.LogWarning("ALERT: Could not find a sprite mask on the question panel!");

            DestroyImmediate(qstnPanel.GetComponent<SpriteRenderer>());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSymbol()
        {
            Transform symbol = qstnPanel.transform.GetChild(symbolChildIndex);

            symbol.localPosition = panelData.symbolOffset;

            symbol.localScale = new Vector2(
                symbol.localScale.x * panelData.symbolScale.x,
                symbol.localScale.y * panelData.symbolScale.y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panelData"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual ClosingArgumentStockBase InitializeStock(ArgumentQuestionPanel panelData, int index)
        {
            ClosingArgumentStockBase currentStock = stockParent.transform.GetChild(index).GetComponent<ClosingArgumentStockBase>();

            currentStock.SetStockVisual(panelData.panelShape);

            currentStock.SetFlavorText(panelData.flavorText);

            currentStock.SetTotalLocks(panelData.noOfLocks);

            currentStock.SetActivePanelScale(panelData.activeScale);

            currentStock.SetActivePanelPosition(panelData.activePosition);

            currentStock.UpdateLockState();

            return currentStock;
        }
    }
}