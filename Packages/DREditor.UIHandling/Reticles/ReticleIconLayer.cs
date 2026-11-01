//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.UIHandling.Reticles
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ReticleIconLayer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string iconBool = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public LayerMask mask;

        public ReticleIconLayer(string iconBool) => this.iconBool = iconBool;
    }
}