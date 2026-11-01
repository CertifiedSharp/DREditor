//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SaveableData
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("DataList")]
        public List<SaveableDataSet> dataSets = new();


        public SaveableData() { }

        public SaveableData(List<SaveableDataSet> dataSets) => this.dataSets = dataSets;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMatch()
        {
            for (int i = 0; i < dataSets.Count; i++)
            {
                SaveableDataSet item = dataSets[i];

                for (int x = 0; x < dataSets.Count; x++)
                {
                    if (i == x)
                        continue;

                    SaveableDataSet info = dataSets[x];

                    if (info.subID == item.subID &&
                        item.typeName == info.typeName)
                    {
                        UnityEngine.Debug.LogWarning("2 Managers of type : " + item.GetType().Name +
                            " contain the same SubIDs! This is not allowed because otherwise the " +
                            "ISaveable will not load Save Data correctly!");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}