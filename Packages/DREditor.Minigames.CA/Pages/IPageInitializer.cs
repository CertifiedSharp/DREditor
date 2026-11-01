// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    public interface IPageInitializer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnedPage"></param>
        /// <param name="totalPages"></param>
        void OnPageSpawned(GameObject spawnedPage, int totalPages);

        /// <summary>
        ///
        /// </summary>
        void SetUpPageDisplay();
    }
}