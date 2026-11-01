//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PresentReactionLevelOption : ReactionImportOption
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Character character;


        /// <inheritdoc/>
        public override void Invoke()
        {
            if (InvalidCharacterSelected())
                return;

            AssetDatabase.StartAssetEditing();

            SetPresentsReactionLevel();

            AssetDatabase.StopAssetEditing();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InvalidCharacterSelected()
        {
            bool result = character == null || string.IsNullOrEmpty(character.FirstName);

            if (result)
            {
                EditorUtility.DisplayDialog("No Character Selected", "Please make sure you select a character to " +
                    "use this functionality!", "OK");

                Close();
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetPresentsReactionLevel()
        {
            int count = 0;

            foreach (Present present in presentDatabase.presents)
            {
                var matches = Regex.Match(textArea, present.DisplayName);

                if (matches.Length > 0)
                {
                    count++;

                    ApplyReactions(present, character.FirstName);
                }
            }

            UnityEngine.Debug.Log("REPORT: Found Presents: " + count);
        }
    }
}
#endif