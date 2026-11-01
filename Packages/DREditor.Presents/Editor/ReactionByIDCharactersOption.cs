//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ReactionByIDCharactersOption : ReactionImportOption
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Header("")]
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected string splitLineRegex = @"(?:^|[,\\s\\t]+)([A-Za-z]+(?:\\s+[A-Za-z]+)*)";


        /// <summary>
        ///
        /// </summary>
        protected List<int> idMatches;


        /// <inheritdoc/>
        public override void Invoke()
        {
            try
            {
                AssetDatabase.StartAssetEditing();

                SetAllOfReactionType();
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError("ALERT: Failed to apply process so it was aborted. \nReason: " + exception.ToString());
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetAllOfReactionType()
        {
            using var stringReader = new StringReader(textArea);

            string line;

            while ((line = stringReader.ReadLine()) != null)
                if (MatchedIDs(line))
                    ApplyMatchedNames(Regex.Split(line, splitLineRegex));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected virtual bool MatchedIDs(string line)
        {
            idMatches = CollectIDs(Regex.Split(line, anyNumberRegex));

            return idMatches != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameMatches"></param>
        protected virtual void ApplyMatchedNames(string[] nameMatches)
        {
            for (int i = 0; i < nameMatches.Length; i++)
                if (i == 0 || TotalFailedMatch(nameMatches[i]))
                    continue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tryMatch"></param>
        /// <returns></returns>
        protected virtual bool TotalFailedMatch(string tryMatch)
        {
            var nameMatch = tryMatch.Replace(",", "");

            if (FailedMatch(nameMatch))
                return true;
            else
                PassedMatch(nameMatch);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameMatch"></param>
        /// <returns></returns>
        protected virtual bool FailedMatch(string nameMatch)
        {
            if (nameMatch == "")
                return true;
            else
                UnityEngine.Debug.Log("REPORT: Name Matched: " + nameMatch);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nameMatch"></param>
        protected virtual void PassedMatch(string nameMatch)
        {
            if (presentDatabase.GetPresentByID(idMatches[0]) is Present present)
                ApplyReactions(present, nameMatch);
            else
                UnityEngine.Debug.LogWarning("ALERT: Couldn't Find Present With ID: " + idMatches[0]);
        }
    }
}
#endif