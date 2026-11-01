//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Room Loading/Past Point Data", fileName = "PastPointData")]
    [Serializable]
    public class PastPointData : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int chapterIndex;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string objective;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool active;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 position;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 rotation;
    }
}