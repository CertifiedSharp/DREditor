//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using DREditor.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public abstract class HistoricalManagerBase<T, T2> : ServiceHolder<T2>,
        IDRESaveable, IDRELoadableManager, IDRELoadable, ILoadOnChapterSelect
        where T2 : class
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEventListener loadProgressionData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener newGameSelected = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string progressionKey = "ProgressionData";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SelectType]
        [SerializeReference] protected T historicalData;


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas;


        protected override void InitializeService()
        {
            base.InitializeService();

            if (ServiceLocator.HasService(out T2 activeManager) && this as T2 == activeManager)
                ServiceLocator.Register<T>(historicalData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (ServiceLocator.HasService(out T activeHistory) && activeHistory.Equals(historicalData))
                ServiceLocator.Unregister<T>(historicalData);
        }


        public virtual void Load(Dictionary<string, object> saveDatas) => this.saveDatas = saveDatas;


        protected virtual void OnEnable()
        {
            loadProgressionData.AddListener(OnLoadProgressionDataInvoked);

            newGameSelected.AddListener(OnNewGameSelected);
        }

        protected virtual void OnDisable()
        {
            loadProgressionData.RemoveListener(OnLoadProgressionDataInvoked);

            newGameSelected.RemoveListener(OnNewGameSelected);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnLoadProgressionDataInvoked() => LoadRegisterData(GetSaveData(saveDatas));

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual T GetSaveData(Dictionary<string, object> saveDatas) =>
            (T)saveDatas.GetGeneric(progressionKey, historicalData.GetType());

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnNewGameSelected()
        {
            if (ServiceLocator.GetService<T>() is IClearableOnNewGameSelected clearable)
                clearable?.Clear();
        }


        public virtual void Save(Dictionary<string, object> saveDatas) =>
            saveDatas.Set(progressionKey, GetInstanceData());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual object GetInstanceData() => historicalData;


        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect) =>
            LoadRegisterData(GetSaveData(saveDatas).DeepCopy());

        /// <summary>
        ///
        /// </summary>
        /// <param name="historicalData"></param>
        protected virtual void LoadRegisterData(T historicalData)
        {
            if (historicalData == null)
                return;

            ServiceLocator.Unregister<T>();

            this.historicalData = historicalData;

            ServiceLocator.Register<T>(this.historicalData);
        }


        public virtual void CarryOverOnChapterSelect(Dictionary<string, object> oldSaveDatas, Dictionary<string, object> newSaveDatas)
        {
            if (GetSaveData(oldSaveDatas) is IPersistentData loadedData && GetSaveData(newSaveDatas) is IPersistentData newData)
                newData?.CarryOver(loadedData);
            else
                OnCarryingOverFailed(oldSaveDatas, newSaveDatas);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="oldSaveDatas"></param>
        /// <param name="newSaveDatas"></param>
        protected virtual void OnCarryingOverFailed(Dictionary<string, object> oldSaveDatas, Dictionary<string, object> newSaveDatas) =>
            UnityEngine.Debug.LogWarning($"ANALYSIS: Either the new or loaded progression data were not of type: {nameof(IPersistentData)}.");
    }
}