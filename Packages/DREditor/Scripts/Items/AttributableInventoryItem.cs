// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Items
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AttributableInventoryItem<T> : InventoryItem where T : IItemAttribute
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeReference]
        [SelectType]
        public List<T> attributes = new();


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public virtual T1 OfType<T1>()
        {
            foreach (var attribute in attributes)
                if (attribute is T1 type && attribute.IsValid())
                    return type;

            return default;
        }
    }
}