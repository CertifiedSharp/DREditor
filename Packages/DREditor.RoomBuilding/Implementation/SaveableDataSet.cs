//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Spawnables;
using System;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SaveableDataSet
    {
        /// <summary>
        ///
        /// </summary>
        public int subID;

        /// <summary>
        ///
        /// </summary>
        [TextArea(3, 1000)]
        public string data;

        /// <summary>
        ///
        /// </summary>
        public string typeName;


        public SaveableDataSet(ISaveable manager)
        {
            if (manager == null)
            {
                UnityEngine.Debug.Log("Logging notice of savable data set with null manager.");

                return;
            }

            subID = manager.SubID;

            data = manager.Save();

            typeName = manager.GetType().Name;
        }
    }
    
    /* The ISaveable is designed to make managers to dynamically save data by transfering it to data strings and 
     * being able to load it whenever one needs it etc.
    public class SaveableClass : Monobehavior, ISaveable
    {
        #region Saving
        public int SubID { get => subID; }
        [SerializeField] int subID;
        public string Save()
        {
            return JsonUtility.ToJson(InstanceData);
        }

        public void Load(string data)
        {
            InstanceData = JsonUtility.FromJson<InstanceDataClass>(data);
        }
        #endregion
    }
    */
}