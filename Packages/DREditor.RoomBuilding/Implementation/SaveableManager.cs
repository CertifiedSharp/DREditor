//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Spawnables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomStateHandlerBase<SaveableStateData, SaveableDataSet>) + 
        " for types <" + nameof(SaveableStateData) + ", " + nameof(SaveableDataSet) + ">" +
        "\n\n")]
    public class SaveableManager : RoomStateHandlerBase<SaveableStateData, SaveableDataSet>
    {
        /// <inheritdoc/>
        public override List<SaveableDataSet> Save() => SaveData().dataSets;

        /// <inheritdoc/>
        public override void Unload(bool inEditor) { }

        /// <inheritdoc/>
        protected override void LoadAssetProcess(List<SaveableDataSet> baseDatas, List<SaveableDataSet> instanceDatas = null)
        {
            Load(new SaveableData(baseDatas));

            Load(new SaveableData(instanceDatas));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static SaveableData SaveData()
        {
            SaveableData data = new();

            var list = UnityEngine.Object.FindObjectsByType<MonoBehaviour>().OfType<ISaveable>();

            if (!list.Any())
                return data;

            foreach (var item in list)
            {
                data.dataSets.Add(new SaveableDataSet(item));
            }

            data.IsMatch();

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        public static void Load(SaveableData data)
        {
            if (data == null || data.dataSets == null || data.dataSets.Count == 0)
                return;

            var list = UnityEngine.Object.FindObjectsByType<MonoBehaviour>().OfType<ISaveable>();

            foreach (var item in list)
                foreach (var info in data.dataSets)
                    if (info.subID == item.SubID && item.GetType().Name == info.typeName)
                        item.Load(info.data);
        }
    }
}