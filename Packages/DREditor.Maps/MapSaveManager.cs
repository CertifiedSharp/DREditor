//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saveables;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISaveManager) +
        "\n\n")]
    public class MapSaveManager : MonoBehaviour, ISaveManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFastTravel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int subID;

        
        public int SubID { get => subID; }


        /// <inheritdoc/>
        public virtual string Save()
        {
            ServiceLocator.GetService<IMapManager>().GetInstanceData().Save();

            MapData mapData = new(ServiceLocator.GetService<IMapManager>().GetInstanceData(), canFastTravel.Value);

            return JsonUtility.ToJson(mapData);
        }

        /// <inheritdoc/>
        public virtual void Load(string data)
        {
            try
            {
                MapData mapData = JsonUtility.FromJson<MapData>(data);

                ServiceLocator.GetService<IMapManager>().SetInstanceData(mapData.Entry);

                canFastTravel.SetValue(mapData.allowFastTravel);
            }
            catch
            {
                ServiceLocator.GetService<IMapManager>().SetInstanceData(JsonUtility.FromJson<MapEntry>(data));
            }
        }

        /// <inheritdoc/>
        public virtual List<string> GetPreviousNames() => new() { "MapManager" };
    }
}