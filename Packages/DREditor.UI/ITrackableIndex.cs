//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public interface ITrackableIndex
    {
        /// <summary>
        ///
        /// </summary>
        Action<int, int> MovedIndexFrom { get; set; }
    }
}