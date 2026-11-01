//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
    public interface IOverlayManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool IsOverlaying();

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        void SetOverlay(bool to);

        /// <summary>
        ///
        /// </summary>
        void ClearOverlayOnActive();
    }
}