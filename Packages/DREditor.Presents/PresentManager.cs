//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using DREditor.Services;
using DREditor.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<IPresentManager>) + " for type: " + nameof(IPresentManager) +
        "\n\n")]
    public class PresentManager : PresentManagerBase<PresentData>, IDRESaveable, IDRELoadableManager, ILoadOnChapterSelect
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveKey = "presentData";


        /// <inheritdoc/>
        public override void AddPresent(int id, int amt = 1)
        {
            IAmountablePresent data = Get<IAmountablePresent>(id);

            if (data != null)
                data.Add(amt);
            else
                presentDatas.Add(new(id, amt));
        }


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) => saveData.Set(saveKey, presentDatas);

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            if (!inChapterSelect)
                SetPresentData(saveDatas.Get<List<PresentData>>(saveKey));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newDatas"></param>
        protected virtual void SetPresentData(List<PresentData> newDatas)
        {
            ClearPresentData();

            if (newDatas != null)
                foreach (var presentData in newDatas)
                    presentDatas.Add(presentData.DeepCopy());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearPresentData() => presentDatas.Clear();


        /// <inheritdoc/>
        public virtual void CarryOverOnChapterSelect(Dictionary<string, object> oldSaveDatas, Dictionary<string, object> newSaveDatas) =>
            newSaveDatas.Set(saveKey, oldSaveDatas.Get<List<PresentData>>(saveKey));
    }
}