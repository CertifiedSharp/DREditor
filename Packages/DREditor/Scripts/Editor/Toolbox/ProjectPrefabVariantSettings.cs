//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectPrefabVariantSettings : ProjectSettingsData<ProjectPrefabVariantSettings>
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public string targetDirectory = "Assets/Prefabs/Project Variants";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public string suffix = "";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public bool permanentlyUseSettings = false;
    }
}