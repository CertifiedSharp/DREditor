//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public class ValueEventHandler<T, TY> : MonoBehaviour, IDRESaveable, IDRELoadableManager where TY : UnityEvent<T>, new()
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<ValueWithKey<T, TY>> entries = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            if (isLoadingFile != null && isLoadingFile.Value)
                return;

            LoadEntries(saveDatas, inChapterSelect);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        /// <param name="inChapterSelect"></param>
        protected virtual void LoadEntries(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            foreach (var entry in entries)
                if (inChapterSelect && entry.skipOnChapterSelect)
                    continue;
                else if (saveDatas.TryGetValue(entry.key, out var val) && val is T typedVal)
                    entry.value.SetValue(typedVal);
                else
                    UnityEngine.Debug.LogWarning($"ALERT: Could not seem to find save data for key: {entry.key}");
        }

        public virtual void Save(Dictionary<string, object> saveData)
        {
            foreach (var entry in entries)
                saveData.Set(entry.key, entry.value.Value);
        }
    }
}