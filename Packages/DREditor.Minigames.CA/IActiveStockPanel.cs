// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    public interface IActiveStockPanel
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="stockIndex"></param>
        void InitializeActivePanel(int stockIndex);

        /// <summary>
        ///
        /// </summary>
        IEnumerator ShootStock();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool CurrentStockSelectable();

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedStock"></param>
        /// <returns></returns>
        bool CurrentStockMatches(Transform selectedStock);
    }
}