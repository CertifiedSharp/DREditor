//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MapData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public MapEntry Entry;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool allowFastTravel = false;


        public MapData() { }

        public MapData(MapEntry Entry, bool allowFastTravel)
        {
            this.Entry = Entry;

            this.allowFastTravel = allowFastTravel;
        }
    }
}