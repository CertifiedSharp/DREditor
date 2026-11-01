//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
    public interface IDoor
    {
        /// <summary>
        ///
        /// </summary>
        void ClearLockData();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        object Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        void Load(object data);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetGameObjectName();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetDoorName();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetSaveDisplayName();

        /// <summary>
        ///
        /// </summary>
        void AddDoorControl();

        /// <summary>
        ///
        /// </summary>
        void RemoveDoorControl();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool IsTPFD();
    }
}