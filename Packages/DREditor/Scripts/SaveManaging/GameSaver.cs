//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using UnityEngine;

namespace DREditor.SaveManaging
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GameSaver : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasLoadedSaveData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent firstTimeLoaded = null;


        /// <summary>
        ///
        /// </summary>
        DRESaveData currentDRESaveData = null;


        /// <summary>
        ///
        /// </summary>
        public DRESaveData CurrentDRESaveData
        {
            get
            {
                return currentDRESaveData;
            }

            set
            {
                currentDRESaveData = value;

                hasLoadedSaveData.SetValue(currentDRESaveData != null);
            }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => CurrentDRESaveData = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public abstract void SaveGameFile(object o = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public virtual void LoadGameFile(object o = null)
        {
            UnityEngine.Debug.Log("File Data is on variable");

            string slotNum = (string)o;

            CurrentDRESaveData = GetData(slotNum);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public abstract void UpdateGameFromData(object o = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns></returns>
        public abstract DRESaveData GetData(string slotNum);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <remarks></remarks>
        public abstract void ApplyCurrentData(object o = null);

        /// <summary>
        ///
        /// </summary>
        public virtual void ClearCurrentData() => CurrentDRESaveData = null;


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => firstTimeLoaded.SetValue(true);
    }
}