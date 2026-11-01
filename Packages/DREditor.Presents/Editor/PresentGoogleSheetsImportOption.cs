//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class PresentGoogleSheetsImportOption : PresentImportOptionBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected virtual List<string> GetListOfNamesGoogleSheetFormat(string pattern)
        {
            var matches = Regex.Matches(textArea, pattern);

            List<string> words = new();

            foreach (Match match in matches)
                if (match.Value == string.Empty || string.IsNullOrWhiteSpace(match.Value))
                    continue;
                else
                    words.Add(match.Value.Replace(tab, ""));

            return words;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="words"></param>
        /// <param name="dictionary"></param>
        protected virtual void CreatePairing(List<int> ids, List<string> words, out Dictionary<int, string> dictionary)
        {
            var count = Math.Min(ids.Count, words.Count);

            dictionary = ids.Take(count)
                .Zip(words.Take(count), (id, word) => new { id, word })
                .ToDictionary(pair => pair.id, pair => pair.word);

            foreach (var pair in dictionary)
                UnityEngine.Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }
    }
}
#endif