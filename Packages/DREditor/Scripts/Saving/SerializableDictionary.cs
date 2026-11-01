//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SerializableDictionary
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<SerializableDictionaryEntry> entries = new();

        public SerializableDictionary(Dictionary<string, object> dict)
        {
            foreach (var kvp in dict)
                entries.Add(new SerializableDictionaryEntry(kvp.Key, kvp.Value));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();

            foreach (var entry in entries)
                dict[entry.key] = entry.GetValue();

            return dict;
        }
    }
}