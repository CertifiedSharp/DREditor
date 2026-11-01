//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.States;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class DifficultyEditorBase<T>
    {
        /// <summary>
        ///
        /// </summary>
        protected StateDatabase difficultyDatabase = null;

        /// <summary>
        ///
        /// </summary>
        protected bool foldout = true;

        /// <summary>
        ///
        /// </summary>
        protected GUIStyle largeFoldoutStyle;


        /// <summary>
        ///
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual T DisplaySettings(T settings)
        {
            if (difficultyDatabase == null)
                difficultyDatabase = BuilderUtility.FindAssetByType<StateDatabase>("Diff");

            InitializeFoldoutStyle();

            return settings;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeFoldoutStyle()
        {
            largeFoldoutStyle ??= new(EditorStyles.foldout)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };
        }
    }
}
#endif