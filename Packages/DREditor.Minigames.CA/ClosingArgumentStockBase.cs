// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ClosingArgumentStockBase : MonoBehaviour, IClosingArgumentResetable, ILockPreview
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int unlockIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int unlockedIndex = -1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int completedIndex = -2;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<ClosingArgumentStockBase> nodeDirections;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("flavourText")]
        public string flavorText;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int remainingLocks;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool selectable = true;

        

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector2 activePanelScale;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector2 activePanelPos;


        /// <summary>
        ///
        /// </summary>
        public bool ToBeUnlocked { get; protected set; } = false;

        /// <summary>
        ///
        /// </summary>
        public bool IsUnlocked { get; protected set; } = false;

        /// <summary>
        ///
        /// </summary>
        public bool Solved { get; protected set; } = false;


        /// <summary>
        ///
        /// </summary>
        protected int totalLocks = -1;


        /// <summary>
        ///
        /// </summary>
        /// <param name="flavorText"></param>
        public virtual void SetFlavorText(string flavorText) => this.flavorText = flavorText;

        /// <summary>
        ///
        /// </summary>
        /// <param name="totalLocks"></param>
        public virtual void SetTotalLocks(int totalLocks)
        {
            this.totalLocks = totalLocks;

            ResetRemainingLocks();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void ResetRemainingLocks() => SetRemainingLocks(totalLocks);

        /// <summary>
        ///
        /// </summary>
        /// <param name="remainingLocks"></param>
        public virtual void SetRemainingLocks(int remainingLocks)
        {
            this.remainingLocks = remainingLocks;

            ToBeUnlocked = remainingLocks == unlockIndex;

            IsUnlocked = remainingLocks == unlockedIndex;

            Solved = remainingLocks == completedIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        public abstract void SetStockVisual(object obj);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract object GetStockVisual();

        /// <summary>
        ///
        /// </summary>
        /// <param name="activePanelScale"></param>
        public virtual void SetActivePanelScale(Vector2 activePanelScale) => this.activePanelScale = activePanelScale;

        /// <summary>
        ///
        /// </summary>
        /// <param name="activePanelPos"></param>
        public virtual void SetActivePanelPosition(Vector2 activePanelPos) => this.activePanelPos = activePanelPos;

        /// <summary>
        ///
        /// </summary>
        public virtual void Solve()
        {
            SetRemainingLocks(completedIndex);

            UpdateLockState();

            UpdateStockLocks();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void UpdateLockState()
        {
            if ((IsUnlocked || ToBeUnlocked) && !Solved)
                OnStockUnlocked();

            selectable = !Solved && (IsUnlocked || ToBeUnlocked);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnStockUnlocked() => SetRemainingLocks(unlockedIndex);


        /// <summary>
        ///
        /// </summary>
        /// <param name="isHovering"></param>
        public abstract void SetHover(bool isHovering);

        /// <summary>
        ///
        /// </summary>
        /// <param name="isClicked"></param>
        public abstract void SetClicked(bool isClicked);

        /// <inheritdoc/>
        public virtual void CAReset() => SetRemainingLocks(completedIndex);

        /// <summary>
        ///
        /// </summary>
        public virtual void UpdateStockLocks()
        {
            //updates the remaining locks across each stock.
            foreach (Transform child in transform.parent)
            {
                ClosingArgumentStockBase childStock = child.GetComponent<ClosingArgumentStockBase>();

                if (childStock.remainingLocks > 0)
                    childStock.SetRemainingLocks(childStock.remainingLocks - 1);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Rewire")]
        public void Rewire()
        {
            nodeDirections.Clear();

            nodeDirections = new(4);

            var directions = RewireUINavigation.FindClosestTransforms(transform);

            foreach (var direction in directions)
                if (direction != null)
                    nodeDirections.Add(direction.GetComponent<ClosingArgumentStockBase>());
                else
                    nodeDirections.Add(null);
        }


        /// <inheritdoc/>
        public virtual void PreviewLockState(bool previewLockState)
        {
            if (!previewLockState)
            {
                SetRemainingLocks(0);

                UpdateLockState();
            }
        }
    }
}