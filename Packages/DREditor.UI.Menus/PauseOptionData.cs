//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Pause Menu/Pause Option Data", fileName = "PauseOptionData")]
    public class PauseOptionData : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public bool[] pauseOptions = new bool[]
        {
            false, false, false, false, false
        };


        /// <summary>
        ///
        /// </summary>
        public virtual void SetPauseOptions(bool[] pauseOptions) => this.pauseOptions = pauseOptions;

        /// <summary>
        ///
        /// </summary>
        public virtual void ClearData()
        {
            for (int i = 0; i < pauseOptions.Length; i++)
                pauseOptions[i] = false;
        }

#if UNITY_EDITOR

        protected virtual void OnEnable() 
            => UnityEditor.EditorApplication.playModeStateChanged += PlayModeStateChange;

        protected virtual void OnDisable() 
            => UnityEditor.EditorApplication.playModeStateChanged -= PlayModeStateChange;

        protected virtual void PlayModeStateChange(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode) 
                ClearData();
        }

#endif
    }
}