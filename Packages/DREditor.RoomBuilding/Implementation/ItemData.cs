//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.LocalDialogues;
using DREditor.Talkables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ItemData
    {
        /// <summary>
        ///
        /// </summary>
        public string iReference;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("iData")]
        public List<LocalDialogue> itemDialogues;

        /// <summary>
        ///
        /// </summary>
        public bool iSelectable;


        public ItemData() { }

        public ItemData(GameObject item, ItemActor itemActor)
        {
            iReference = item.name;

            iSelectable = itemActor.selectable;

            itemDialogues = itemActor.conversations;
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            ItemData newData = new();

            ItemData copy = (ItemData)MemberwiseClone();

            newData.iReference = copy.iReference;

            newData.iSelectable = copy.iSelectable;

            newData.itemDialogues = new List<LocalDialogue>();

            foreach (LocalDialogue localDialogue in copy.itemDialogues)
                newData.itemDialogues.Add((LocalDialogue)localDialogue.Clone());

            return newData;
        }
    }
}