//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    public interface ITPFDListener
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="directionIndex"></param>
        void OnEnteredBoundary(int directionIndex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="directionIndex"></param>
        void OnExitedBoundary(int directionIndex);
    }
}