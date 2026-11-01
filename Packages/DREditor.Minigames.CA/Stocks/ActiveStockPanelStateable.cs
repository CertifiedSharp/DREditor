// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DREditor.Minigames.CA.Stocks
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ActiveStockPanelStateable : ActiveStockPanelBase, ILockPreview
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference move = null;


        [Header("")]
        [SerializeField] protected UnityEvent OnStockScrolled;


        [Header("")]
        [SerializeField] protected UnityEvent OnStockUnlocked;


        /// <inheritdoc/>
        public override void ActivateControls() => move.action.performed += StockMovement;

        /// <inheritdoc/>
        public override void DeactivateControls() => move.action.performed -= StockMovement;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void StockMovement(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();

            int dirIndex;

            if (input.x == 0)
                dirIndex = (input.y > 0) ? 0 : 2; // Up and Down.
            else
                dirIndex = (input.x > 0) ? 1 : 3; // Right and Left.

            if (currentStock.nodeDirections[dirIndex] == null) // if direction pressed leads to null pointer.
                return;

            OnStockUpdated(currentStock.nodeDirections[dirIndex]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newStock"></param>
        protected virtual void OnStockUpdated(ClosingArgumentStockBase newStock)
        {
            currentStock.SetHover(false);

            SetCurrentStock(newStock);

            currentStock.SetHover(true);

            UpdatePanelState();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdatePanelState()
        {
            OnStockScrolled?.Invoke();
            
            // Change activePanel visuals based on status of hovered stock
            if (currentStock.selectable) // stock is unlocked and unsolved
                OnStockUnsolved();
            else if (currentStock.Solved)
                OnStockSolved();
            else if (currentStock.ToBeUnlocked)
                OnStockUnlock();
            else
                OnStockLocked();
        }


        /// <summary>
        ///
        /// </summary>
        protected abstract void OnStockLocked();

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnStockUnsolved();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnStockUnlock() => OnStockUnlocked?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnStockSolved();


        /// <inheritdoc/>
        public override void OnStockCorrect(Transform hoveredObject) => currentStock.Solve();

        /// <inheritdoc/>
        public override void InitializeActivePanel(int stockIndex)
        {
            base.InitializeActivePanel(stockIndex);

            InitializePanelState();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePanelState()
        {
            if (currentStock.ToBeUnlocked ||
                currentStock.IsUnlocked ||
                currentStock.Solved)
                OnStockUnsolved();
            else
                OnStockLocked();
        }


        /// <inheritdoc/>
        public virtual void PreviewLockState(bool previewLockState)
        {
            if (!previewLockState)
                OnStockUnsolved();
        }
    }
}