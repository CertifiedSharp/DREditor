//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MarkerData : MapBaseData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 localPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector2 sizeDelta = Vector2.zero;
    }
}