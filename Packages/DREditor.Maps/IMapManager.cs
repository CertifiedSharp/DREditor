//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public interface IMapManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        MapEntry GetInstanceData();

        /// <summary>
        ///
        /// </summary>
        /// <param name="InstanceData"></param>
        void SetInstanceData(MapEntry InstanceData);

        /// <summary>
        ///
        /// </summary>
        /// <param name="areaName"></param>
        void AddUnlockedArea(string areaName);
    }
}