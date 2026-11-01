//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RenamePresentsByIDOption : PresentGoogleSheetsImportOption
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Header("")]
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected string googleSheetsRegex = "(\\t\\D\\d\\D+|\\D+)|(\\t\\D+|\\D+)";


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<int, string> entries;


        /// <inheritdoc/>
        public override void Invoke() => RenamePresentsByID();

        /// <summary>
        ///
        /// </summary>
        protected virtual void RenamePresentsByID()
        {
            List<int> ids = GetListOfIDs();

            List<string> words = GetListOfNamesGoogleSheetFormat(googleSheetsRegex);

            if (ids.Count == words.Count)
                RenamePresents(ids, words);
            else
                OnFailedRename();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnFailedRename()
        {
            EditorUtility.DisplayDialog("Sheet Format Incorrect: Inconsistent ID's and Name count",
                "Please Copy each column on your google sheets with the ID and Name to use this functionality!", "OK");

            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="words"></param>
        protected virtual void RenamePresents(List<int> ids, List<string> words)
        {
            CreatePairing(ids, words, out entries);

            AssetDatabase.StartAssetEditing();

            foreach (Present present in presentDatabase.presents)
                if (entries.ContainsKey(present.ID))
                    RenamePresent(present);
                else
                    UnityEngine.Debug.LogWarning("ALERT: Couldn't Find Present in dictionary With ID: " + present.ID);

            AssetDatabase.StopAssetEditing();

            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="present"></param>
        protected virtual void RenamePresent(Present present)
        {
            present.DisplayName = entries[present.ID].Trim().Replace("\r", "");

            EditorUtility.SetDirty(present);
        }
    }
}
#endif