//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FreeTimeSurfacing;
using DREditor.ItemRolling;
using DREditor.Items;
using DREditor.Processables;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.GachaSurface
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FTESurfaceComponent) + ", " + nameof(IItemRollerResponse) + ", " +
        nameof(IStartUpable) + ", " + nameof(ISubmittable) +
        "\n\n")]
    public class ItemRolledResponse : FTESurfaceComponent, IItemRollerResponse, IStartUpable, ISubmittable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("presentImage")]
        [SerializeField] protected Image itemImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite transparentSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("presentNameText")]
        [SerializeField] protected TextMeshProUGUI itemNameText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string presentNameSuffix = "!";


        /// <summary>
        ///
        /// </summary>
        protected IItemRoller itemRoller;

        /// <summary>
        ///
        /// </summary>
        protected IInventoryItem rolledItem;


        /// <inheritdoc/>
        public virtual void StartUp()
        {
            if (itemImage != null && transparentSprite != null)
                itemImage.sprite = transparentSprite;
        }

        /// <inheritdoc/>
        public virtual void InitializeForResponse(IItemRoller itemRoller) => this.itemRoller = itemRoller;

        /// <inheritdoc/>
        public virtual void Submit(Action onComplete)
        {
            CheckItemResponse();

            onComplete?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckItemResponse()
        {
            if (SuccessfullyRolledItem())
                DisplayItemData();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SuccessfullyRolledItem() => itemRoller.TryGetRolledObject(out rolledItem);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayItemData()
        {
            itemImage.sprite = rolledItem.Image;

            itemNameText.text = rolledItem.DisplayName + presentNameSuffix;
        }
    }
}