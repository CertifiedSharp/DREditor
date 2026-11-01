//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SimpleFTEEntryCollection : ICharacterData
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [TextArea]
        [SerializeField] protected List<string> entries = new();

        public virtual int Count => entries.Count;

        public virtual bool IsValid => Count != 0;

        public string this[int i] => entries[i];
    }
}