// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using UnityEngine;

namespace DREditor.Items
{
    /// <summary>
    ///
    /// </summary>
    public interface IInventoryItem : INameDisplayable, IDescriptable
    {
        /// <summary>
        ///
        /// </summary>
        Sprite Image { get; set; }
    }
}