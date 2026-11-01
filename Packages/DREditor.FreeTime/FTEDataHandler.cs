//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Saving;
using DREditor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadableManager) + ", " + 
        nameof(IFTEDataHandler) + ", " + nameof(ILoadOnChapterSelect) + 
        "\n\n")]
    public class FTEDataHandler : MonoBehaviour, IDRESaveable, IDRELoadableManager, IFTEDataHandler, ILoadOnChapterSelect
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string fTEDataSaveKey = "fTEDatas";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected CharacterDatabase characterDatabase;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<FTEData> fTEDatas = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => InitializeFTEData();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeFTEData()
        {
            if (characterDatabase != null && fTEDatas.Count != characterDatabase.GetFTECharacterCount())
            {
                ClearData();

                foreach (Character character in characterDatabase.GetFTECharacterList())
                    AddCharacter(character.FirstName);
            }
        }


        /// <inheritdoc/>
        public virtual void AddCharacter(string name)
        {
            var data = ScriptableObject.CreateInstance<FTEData>();

            data.SetData(name);

            fTEDatas.Add(data);
        }

        /// <inheritdoc/>
        public virtual int GetLevelOfCharacter(string name)
        {
            foreach (FTEData data in fTEDatas)
                if (data.name == name)
                    return data.level;

            UnityEngine.Debug.LogWarning("WARNING: Could not find FTE Level for character: " + name);

            return -1;
        }

        
        /// <inheritdoc/>
        public virtual void LevelUpCharacter(string name)
        {
            var datas = fTEDatas.Where(character => character.name == name);

            if (datas.Count() > 0)
                fTEDatas[fTEDatas.IndexOf(datas.ElementAt(0))].AddLevel();
            else
                UnityEngine.Debug.LogWarning("WARNING: Could not find character: " + name + " to level up.");
        }

        /// <inheritdoc/>
        public virtual int GetAffinity(string name)
        {
            int characterLevel = GetLevelOfCharacter(name);

            if (characterLevel >= 0)
                return characterLevel;

            AddCharacter(name);

            return GetAffinity(name);
        }


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) => saveData.Set(fTEDataSaveKey, fTEDatas);


        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            var loadedDatas = saveDatas.Get<List<FTEData>>(fTEDataSaveKey);

            if (loadedDatas == null || fTEDatas == loadedDatas || loadedDatas.Count == 0)
                return;
            else if (!inChapterSelect)
                LoadFTEDatas(loadedDatas);
            else
                ValidateData();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadedDatas"></param>
        protected virtual void LoadFTEDatas(List<FTEData> loadedDatas)
        {
            ClearData();

            CopyFTEData(loadedDatas);

            ValidateData();

            RemoveEmptyInstances();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loadedDatas"></param>
        protected virtual void CopyFTEData(List<FTEData> loadedDatas)
        {
            foreach (var data in loadedDatas)
                fTEDatas.Add(data.DeepJsonCopy());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ValidateData()
        {
            foreach (var fTEData in fTEDatas)
                fTEData.Validate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveEmptyInstances() => fTEDatas.RemoveAll(item => string.IsNullOrEmpty(item.name));


        /// <inheritdoc/>
        public virtual void ClearData() => fTEDatas.Clear();


        /// <inheritdoc/>
        public virtual void CarryOverOnChapterSelect(Dictionary<string, object> oldSaveDatas, Dictionary<string, object> newSaveDatas) =>
            newSaveDatas.Set(fTEDataSaveKey, oldSaveDatas.Get<List<FTEData>>(fTEDataSaveKey));
    }
}