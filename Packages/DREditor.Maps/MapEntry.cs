//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MapEntry
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public MapObjectiveData IdentifierData;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<string> UnlockedAreas = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<MapAreaEntry> AreaEntries = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="UnlockedAreas"></param>
        public virtual void SetUnlockedAreas(List<string> UnlockedAreas) => this.UnlockedAreas = UnlockedAreas;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual MapAreaEntry GetAreaEntry(string name)
        {
            if (AreaEntries.Where(e => e.Data.name == name).ToList() is List<MapAreaEntry> list && 
                list.Count > 0)
                return list[0];

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Save()
        {
            foreach(var areaEntry in AreaEntries)
                areaEntry.Save();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="areaData"></param>
        /// <returns></returns>
        public virtual MapAreaEntry Contains(MapAreaData areaData)
        {
            for (int i = 0; i < AreaEntries.Count; i++)
                if (AreaEntries[i].Data == areaData)
                    return AreaEntries[i];

            return null;
        }
    }
}