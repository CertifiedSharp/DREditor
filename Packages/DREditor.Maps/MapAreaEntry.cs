//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MapAreaEntry
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public MapBaseData Data;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<PlaceEntry> PlaceEntries = new();


        public MapAreaEntry() { }

        public MapAreaEntry(MapBaseData Data) => this.Data = Data;


        /// <summary>
        ///
        /// </summary>
        public virtual void Save()
        {
            foreach(PlaceEntry entry in PlaceEntries)
                entry.Save();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual PlaceEntry ContainsPlace(string id)
        {
            for (int i = 0; i < PlaceEntries.Count; i++)
                if (PlaceEntries[i].GetID() == id)
                    return PlaceEntries[i];

            return null;
        }
    }
}