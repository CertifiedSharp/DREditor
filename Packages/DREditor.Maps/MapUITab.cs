//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MapUITab : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        public abstract void InitializeTab();

        /// <summary>
        ///
        /// </summary>
        public abstract void ShowTab();

        /// <summary>
        ///
        /// </summary>
        public abstract void HideTab();
    }
}