//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PauseOptionSaveHandler : MonoBehaviour, IDRESaveable, IDRELoadableManager, IPlayerDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKey = "pauseOptions";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PauseOptionData pauseData = null;


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            string serializedPauseOptions = JsonUtility.ToJson(pauseData, true);

            saveData.Set(saveKey, serializedPauseOptions);
        }

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            try
            {
                pauseData.SetPauseOptions(saveDatas.Get<bool[]>(saveKey));

                if (pauseData.pauseOptions.Length == 0)
                    NormallyLoadPauseOptions(saveDatas);
            }
            catch
            {
                NormallyLoadPauseOptions(saveDatas);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        protected virtual void NormallyLoadPauseOptions(Dictionary<string, object> saveDatas)
        {
            try
            {
                UnityEngine.Debug.Log("REPORT: Old Pause Data not found, loading normally...");

                JsonUtility.FromJsonOverwrite(saveDatas.Get<string>(saveKey), pauseData);

                UnityEngine.Debug.Log("REPORT: Pause Options Successfully Loaded.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("ALERT: Loading Pause Options Critically Failed.");

                UnityEngine.Debug.LogWarning(ex.ToString());
            }
        }


        /// <inheritdoc/>
        public virtual void ClearData()
        {
            if (pauseData != null)
                pauseData.ClearData();
        }
    }
}