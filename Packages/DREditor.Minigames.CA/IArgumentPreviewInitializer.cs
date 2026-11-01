// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    public interface IArgumentPreviewInitializer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="currentPage"></param>
        /// <param name="stockIndex"></param>
        void InitializePreview(ScriptableObject builder, int currentPage, int stockIndex);
    }
}