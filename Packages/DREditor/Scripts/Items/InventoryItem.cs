// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Items
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Items/Simple Inventory Item", fileName = "Item")]
    public class InventoryItem : ScriptableObject, IInventoryItem
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("Title")]
        [SerializeField] protected string displayName;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("Description")]
        [TextArea]
        [SerializeField] protected string description;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("Picture")]
        [PreviewSprite]
        [SerializeField] protected Sprite image;


        /// <inheritdoc/>
        public virtual string DisplayName { get => displayName; set => displayName = value; }

        /// <inheritdoc/>
        public virtual string Description { get => description; set => description = value; }

        /// <inheritdoc/>
        public virtual Sprite Image { get => image; set => image = value; }
    }
}