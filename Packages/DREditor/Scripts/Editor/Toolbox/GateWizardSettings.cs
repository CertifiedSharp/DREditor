//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class GateWizardSettings : ProjectSettingsData<GateWizardSettings>
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public string targetDirectory = "Resources/Gates";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public string scenePrefix = "GYM_";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public string assetPrefix = "G_";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public bool permanentlyUseSettings = false;
    }
}