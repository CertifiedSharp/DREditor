//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.RoomStateBuilding
{
    /// <summary>
    ///
    /// </summary>
    public interface IRoomStateSaver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ScriptableObject SaveDataAsset(string path);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        object SaveInstance();
    }
}