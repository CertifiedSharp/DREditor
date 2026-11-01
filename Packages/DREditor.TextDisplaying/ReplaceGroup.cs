//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ReplaceGroup
    {
        public ReplaceGroup() { }

        public ReplaceGroup(string findValue, string replacedWithValue)
        {
            this.findValue = findValue;

            this.replacedWithValue = replacedWithValue;
        }

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string findValue;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string replacedWithValue;
    }
}