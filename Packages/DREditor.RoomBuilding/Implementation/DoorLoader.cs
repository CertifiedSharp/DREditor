//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Doors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<DoorStateData, DoorData>) + 
        " for types <" + nameof(DoorStateData) + ", " + nameof(DoorData) + ">" +
        "\n\n")]
    public class DoorLoader : RoomStateHandlerBase<DoorStateData, DoorData>
    {
        /// <inheritdoc/>
        public override List<DoorData> Save() => SaveData();

        /// <inheritdoc/>
        public override void Unload(bool inEditor) => UnloadDoors();

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<DoorData> baseDatas, List<DoorData> instanceDatas = null) 
            => Load(baseDatas, instanceDatas);

        /// <inheritdoc/>
        protected override bool LoadedInstanceValid(string dataInstance, out DoorData result)
            => base.LoadedInstanceValid(dataInstance, out result) && !string.IsNullOrEmpty(result.doorName);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<DoorData> SaveData()
        {
            List<DoorData> doorData = new();

            var doorList = GetDoors();

            foreach (IDoor door in doorList)
                doorData.Add((DoorData)door.Save());

            return doorData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadedData"></param>
        /// <param name="instanceData"></param>
        public static void Load(List<DoorData> loadedData, List<DoorData> instanceData = null)
        {
            var doorList = GetDoors();

            for (int i = 0; i < loadedData.Count; i++)
                for (int x = 0; x < doorList.Count(); x++)
                {
                    if (doorList[x].GetGameObjectName() == loadedData[i].doorName)
                    {
                        DoorData doorInstance = GetInstanceData(doorList[x].GetGameObjectName(), instanceData);

                        var doorDataToLoad = doorInstance != null
                            ? MergeInstance(loadedData[i], doorInstance)
                            : (DoorData)loadedData[i].Clone(); // fallback to base data

                        doorList[x].Load(doorDataToLoad);
                    }
                }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doorName"></param>
        /// <param name="instanceDatas"></param>
        /// <returns></returns>
        static DoorData GetInstanceData(string doorName, List<DoorData> instanceDatas)
        {
            if (instanceDatas != null)
                return instanceDatas.FirstOrDefault(doorData => doorData.doorName == doorName);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doorBaseData"></param>
        /// <param name="doorInstaceData"></param>
        /// <returns></returns>
        static DoorData MergeInstance(DoorData doorBaseData, DoorData doorInstaceData)
        {
            DoorData copy = (DoorData)doorBaseData.Clone();

            copy.SetIdentified(doorInstaceData.identified);

            return copy;
        }

        /// <summary>
        ///
        /// </summary>
        public static void UnloadDoors()
        {
            var doors = GetDoors();

            foreach (IDoor door in doors)
                door.ClearLockData();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        static IDoor[] GetDoors() => FindObjectsByType<MonoBehaviour>().OfType<IDoor>().ToArray();
    }
}