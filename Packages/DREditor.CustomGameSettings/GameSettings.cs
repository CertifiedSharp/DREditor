//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.CustomSettings
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int frameRate = 60;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int vSyncCount = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<CustomGameSettings> customSettings = new();


        public GameSettings() { }

        public GameSettings(int frameRate, int vSyncCount, List<CustomGameSettings> customSettings)
        {
            this.frameRate = frameRate;

            this.vSyncCount = vSyncCount;

            this.customSettings = customSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customSettings"></param>
        public virtual void SetCustomSettings(List<CustomGameSettings> customSettings) => this.customSettings = customSettings;
    }
}