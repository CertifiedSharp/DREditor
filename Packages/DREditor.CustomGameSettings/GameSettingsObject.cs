//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.CustomSettings
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Game Settings/Custom Settings", fileName = "GameSettings")]
    public class GameSettingsObject : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]

        [Space(10)]

        [Header("")]
        [Divider]
        [Tooltip("")]
        public int defaultFrameRate = 60;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int defaultVSyncCount = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<CustomGameSettings> defaultCustomSettings = new();


        /// <summary>
        ///
        /// </summary>
        public virtual GameSettings ToGameSettings() => new(defaultFrameRate, defaultVSyncCount, defaultCustomSettings);
    }
}