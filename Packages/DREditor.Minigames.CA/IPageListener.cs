// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    public interface IPageListener
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        void OnPageUpdated(int currentPage, int totalPages);
    }
}