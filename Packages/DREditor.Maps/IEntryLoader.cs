//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine.UI;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public interface IEntryLoader
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="markers"></param>
        void LoadEntry(object entry, List<Image> markers);
    }
}