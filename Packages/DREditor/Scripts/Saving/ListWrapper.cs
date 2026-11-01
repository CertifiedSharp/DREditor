//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ListWrapper
    {
        /// <summary>
        ///
        /// </summary>
        public List<SerializedItem> items = new();

        public ListWrapper(System.Collections.IList list)
        {
            foreach (var item in list)
            {
                if (item == null)
                {
                    items.Add(new SerializedItem(null, null));

                    continue;
                }

                string itemType = item.GetType().AssemblyQualifiedName;

                string itemValue;

                if (item is int || item is float || item is string || item is bool)
                    itemValue = item.ToString();
                else if (item.GetType().IsSerializable)
                    itemValue = JsonUtility.ToJson(item);
                else
                {
                    UnityEngine.Debug.LogWarning($"Type {item.GetType()} is not serializable.");

                    itemValue = null;
                }

                items.Add(new SerializedItem(itemType, itemValue));
            }
        }

        public ListWrapper(Array array)
        {
            foreach (var item in array)
            {
                if (item == null)
                {
                    items.Add(new SerializedItem(null, null));

                    continue;
                }

                string itemType = item.GetType().AssemblyQualifiedName;

                string itemValue;

                if (item is int || item is float || item is string || item is bool)
                    itemValue = item.ToString();
                else if (item.GetType().IsSerializable)
                    itemValue = JsonUtility.ToJson(item);
                else
                {
                    UnityEngine.Debug.LogWarning($"{ToString()}: Type {item.GetType()} is not serializable.");

                    itemValue = null;
                }

                items.Add(new SerializedItem(itemType, itemValue));
            }
        }
    }
}