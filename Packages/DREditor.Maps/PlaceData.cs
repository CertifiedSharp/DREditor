//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PlaceData : DiscoverableBase
    {
        /// <summary>
        ///
        /// </summary>
        public bool fastTravelable = false;

        /// <summary>
        ///
        /// </summary>
        public string sceneName = "";

        /// <summary>
        ///
        /// </summary>
        public int fastTravelIndex = -1;
    }
}