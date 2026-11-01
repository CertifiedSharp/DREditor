//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.RoomBuilding.Old;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomBuilding.Instances
{
    /// <summary>
    ///
    /// </summary>
    public static class ConvertInstanceData
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static InstanceStateData Convert(InstanceData data)
        {
            InstanceStateData stateData = new(data.CurrentObjective, ConvertRoom(data.CurrentRoom));

            return stateData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static RoomInstanceData ConvertRoom(Room room)
            => new(room.Name, room.Data.isPersistent, room.Data.progressionValue, ConvertInstanceDatas(room));

        /// <summary>
        ///
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static string ConvertInstanceDatas(Room room) => ConvertInstanceRoomDatas(room.Data);

        /// <summary>
        ///
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static string ConvertInstanceRoomDatas(RoomData room)
        {
            InstanceListWrapper<InstanceListWrapper<string>> instanceTemp = new();

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.spawnDatas)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.actorDatas)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.itemDatas)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.triggers)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.subAreas)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.doorDatas)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.moveDatas)));

            instanceTemp.wrappedInstances.Add(new InstanceListWrapper<string>(SerializeString(room.saveables.dataSets)));

            return JsonUtility.ToJson(instanceTemp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<string> SerializeString<T>(List<T> data)
        {
            List<string> strings = new();

            foreach (T data2 in data)
            {
                string serializedJson = JsonUtility.ToJson(data2);

                strings.Add(serializedJson);

                UnityEngine.Debug.LogWarning(serializedJson);
            }

            return strings;
        }
    }
}