//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Minigames.CA
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ClosingArgumentSettings : ProjectSettingsData<ClosingArgumentSettings>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject previewPrefab = null;

        /// <summary>
        ///
        /// </summary>
        public virtual GameObject PreviewPrefab { get => previewPrefab; set => previewPrefab = value; }
    }
}