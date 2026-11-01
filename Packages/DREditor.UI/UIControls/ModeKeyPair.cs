//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ModeKeyPair
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string key;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public BoolWithEvent mode;
    }
}