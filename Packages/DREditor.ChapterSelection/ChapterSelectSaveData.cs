//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.Progressing;
using DREditor.Saving;
using System;
using System.Collections.Generic;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ChapterSelectSaveData : IChapterSelectData, IIdentifiableValidator, IPersistentData, IClearableOnNewGameSelected
    {
        public List<int> unlockedScenes = new();

        public ChapterSelectSaveData() { }


        /// <inheritdoc/>
        public virtual void Add(object obj)
        {
            if (obj is int ID)
                UnlockSceneID(ID);
            else
                OnUnexpectedTypeAdded(obj);
        }

        protected virtual void OnUnexpectedTypeAdded(object obj) =>
            UnityEngine.Debug.LogWarning($"ALERT: ");

        protected virtual void UnlockSceneID(int ID)
        {
            if (!IDValid(ID))
                unlockedScenes.Add(ID);
        }

        /// <inheritdoc/>
        public virtual bool IDValid(int ID) => unlockedScenes.Contains(ID);

        /// <inheritdoc/>
        public virtual void CarryOver(IPersistentData persistentData)
        {
            if (persistentData is ChapterSelectSaveData oldData)
                unlockedScenes = oldData.unlockedScenes;
            else
                UnityEngine.Debug.LogWarning("ALERT: TRIED TO SET CHAPTER SELECT DATA OF A DIFFERENT TYPE!");
        }

        /// <inheritdoc/>
        public virtual void Clear() => unlockedScenes.Clear();
    }
}