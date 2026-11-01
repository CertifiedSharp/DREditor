//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Doors;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public static class DoorLoaderUtility
    {
#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Doors/Apply Current Door Names to Saves")]
        public static void SetAllDoorsToName()
        {
            List<IDoor> doors = MonoBehaviour.FindObjectsByType<MonoBehaviour>().OfType<IDoor>().ToList();

            List<DoorStateData> datas = BuilderUtility.FindAssetsByType<DoorStateData>();

            foreach (IDoor door in doors)
                foreach (var doorStateData in datas)
                    foreach (var doorData in doorStateData.datas)
                        if (doorData.doorName == door.GetDoorName())
                            SetDisplayName(doorStateData, doorData, door);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doorStateData"></param>
        /// <param name="data"></param>
        /// <param name="door"></param>
        static void SetDisplayName(DoorStateData doorStateData, DoorData data, IDoor door)
        {
            data.SetDisplayName(door.GetSaveDisplayName());

            EditorUtility.SetDirty(doorStateData);
        }

#endif
    }
}
#endif