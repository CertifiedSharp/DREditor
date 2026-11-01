//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [CreateAssetMenu(menuName = "DREditor/Map/Marker Vessel", fileName = "Marker_Vessel")]
    [Serializable]
    public class MarkerVessel : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public MarkerData markerData = new();

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("texture")]
        [Tooltip("")]
        public Sprite sprite;


        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        public virtual void SetTexture(Sprite sprite) => this.sprite = sprite;
    }
}