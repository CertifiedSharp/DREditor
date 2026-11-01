//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Saveables
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<SaveManagersHandler>) + " for type: " + nameof(SaveManagersHandler) +
        "\n\n")]
    public class SaveManagersHandler : ServiceHolder<SaveManagersHandler>, IDRESaveable, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKey = "ManagerData";

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        protected virtual void LoadManagerData(ManagerData data)
        {
            if (data == null)
                return;

            var list = FindObjectsByType<MonoBehaviour>().OfType<ISaveManager>();

            foreach (var item in list)
                foreach (var info in data.dataSets)
                    if (info.subID == item.SubID &&
                        (item.GetType().Name == info.typeName || item.GetPreviousNames().Contains(info.typeName)))
                        item.Load(info.data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual ManagerData CreateManagerData()
        {
            ManagerData data = new();

            var list = FindObjectsByType<MonoBehaviour>().OfType<ISaveManager>();

            foreach (var manager in list)
                if (manager != null)
                    data.dataSets.Add(new ManagerDataSet(manager));

            data.HasDuplicate();

            return data;
        }


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) 
            => saveData.Set(saveKey, CreateManagerData());

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect) 
            => LoadManagerData(saveDatas.Get<ManagerData>(saveKey));
    }
}