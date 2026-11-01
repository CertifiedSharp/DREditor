//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class DRESaveData
    {
        public DRESaveData()
        {
            saveDataEntries = new();
        }

        public DRESaveData(Dictionary<string, object> d)
        {
            saveDataEntries = d;
        }

        /// <summary>
        ///
        /// </summary>
        public int version;

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public Dictionary<string, object> saveDataEntries = new();
    }
}