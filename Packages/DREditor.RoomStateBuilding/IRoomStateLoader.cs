//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomStateBuilding
{
    /// <summary>
    ///
    /// </summary>
    public interface IRoomStateLoader
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="roomSO"></param>
        void LoadData(List<ScriptableObject> roomSO);

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomSO"></param>
        /// <param name="instanceData"></param>
        void LoadInstance(List<ScriptableObject> roomSO, object instanceData);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inEditor"></param>
        void Unload(bool inEditor);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetTypeName();
    }
}