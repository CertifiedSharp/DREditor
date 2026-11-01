// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.CA.Stocks
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ActiveStockPanelBase : ControlledComponent, IClosingArgumentListener, IActiveStockPanel
    {
        /// <summary>
        ///
        /// </summary>
        protected ClosingArgumentStockBase firstStock;

        /// <summary>
        ///
        /// </summary>
        protected ClosingArgumentStockBase currentStock;


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentStock"></param>
        protected virtual void SetCurrentStock(ClosingArgumentStockBase currentStock) => this.currentStock = currentStock;


        /// <inheritdoc/>
        public virtual void InitializeActivePanel(int stockIndex)
        {
            var parentStock = transform.parent.GetComponentInChildren<ClosingArgumentStockBase>();

            if (parentStock == null)
                return;

            var stockParent = parentStock.transform.parent;

            var newStock = stockParent.transform.GetChild(stockIndex).GetComponent<ClosingArgumentStockBase>();

            firstStock = newStock;

            SetCurrentStock(newStock);

            currentStock.SetHover(true);

            currentStock.UpdateLockState();
        }

        /// <inheritdoc/>
        public virtual IEnumerator ShootStock()
        {
            currentStock.SetClicked(true);

            yield return OnStockShot();

            currentStock.SetClicked(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator OnStockShot();

        /// <inheritdoc/>
        public virtual bool CurrentStockSelectable() => currentStock.selectable;

        /// <inheritdoc/>
        public virtual bool CurrentStockMatches(Transform selectedStock) => selectedStock == currentStock.transform;


        /// <inheritdoc/>
        public abstract void OnStockCorrect(Transform hoveredObject);

        /// <inheritdoc/>
        public virtual void CAReset() => SetCurrentStock(firstStock);
    }
}