//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class DescriptionsByIDOption : PresentGoogleSheetsImportOption
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Header("")]
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected string googleSheetsRegex = "\\t(.+)";


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<int, string> entries;


        /// <inheritdoc/>
        public override void Invoke()
        {
            List<int> ids = CollectIDs(GetMatches());

            List<string> words = GetListOfNamesGoogleSheetFormat(googleSheetsRegex);

            if (HasInvalidPairCount(ids.Count != words.Count))
                return;

            CreatePairing(ids, words, out entries);

            AssetDatabase.StartAssetEditing();

            ImportDescriptions();

            AssetDatabase.StopAssetEditing();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string[] GetMatches()
        {
            var matched = Regex.Matches(textArea, "\\d+\\t");

            return matched.Select(m => m.Value).ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual bool HasInvalidPairCount(bool condition)
        {
            if (condition)
            {
                EditorUtility.DisplayDialog("Sheet Format Incorrect: Inconsistent " +
                    "ID's and Name count", "Please Copy each column on your google sheets with the ID and Name to" +
                    " use this functionality!", "OK");

                Close();
            }

            return condition;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ImportDescriptions()
        {
            foreach (Present present in presentDatabase.presents)
            {
                if (entries.ContainsKey(present.ID))
                {
                    if (entries[present.ID] == present.DisplayName)
                        continue;

                    present.Description = entries[present.ID];

                    EditorUtility.SetDirty(present);
                }
                else
                    UnityEngine.Debug.Log("Couldn't find import data to match with ID: " + present.ID);
            }
        }
    }
}
#endif