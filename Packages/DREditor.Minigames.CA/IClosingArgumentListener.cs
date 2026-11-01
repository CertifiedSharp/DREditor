// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    public interface IClosingArgumentListener : IClosingArgumentResetable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="hoveredObject"></param>
        void OnStockCorrect(Transform hoveredObject);
    }
}