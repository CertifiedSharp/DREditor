//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    public interface IFTESurfaceManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        FTESurfaceComponent GetSurfaceAt(int index);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        FTESurfaceComponent GetSelectedSurface();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetSelectionHandler<T>(out T value);
    }
}