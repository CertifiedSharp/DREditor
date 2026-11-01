//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public interface IGridIconSlot
    {
        /// <summary>
        ///
        /// </summary>
        void ClearSlotState();

        /// <summary>
        ///
        /// </summary>
        /// <param name="globalIndex"></param>
        /// <param name="selected"></param>
        void UpdateSlot(int globalIndex, bool selected);
    }
}