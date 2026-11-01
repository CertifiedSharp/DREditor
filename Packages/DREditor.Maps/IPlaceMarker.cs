//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public interface IPlaceMarker : IEntryLoader
    {
        /// <summary>
        ///
        /// </summary>
        void Select();

        /// <summary>
        ///
        /// </summary>
        void Deselect();

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        void Initialize(object data);
    }
}