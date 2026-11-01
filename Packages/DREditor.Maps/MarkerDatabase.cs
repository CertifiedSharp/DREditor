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
    [CreateAssetMenu(menuName = "DREditor/Map/Marker Database", fileName = "Marker_Database")]
    [Serializable]
    public class MarkerDatabase : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<MarkerVessel> Entries = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual MarkerVessel GetVesselByData(MarkerData data)
        {
            if (Entries.Where(e => e.markerData.name == data.name).ToList() is List<MarkerVessel> list &&
                list.Count > 0)
                return list[0];

            if (Entries.Where(e => e.markerData.GetID() == data.GetID()).ToList() is List<MarkerVessel> idList &&
                idList.Count > 0)
                return idList[0];

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetVesselNames()
        {
            string[] names = new string[Entries.Count];

            for(int i = 0; i < Entries.Count; i++)
                names[i] = Entries[i].name;

            return names;
        }
    }
}