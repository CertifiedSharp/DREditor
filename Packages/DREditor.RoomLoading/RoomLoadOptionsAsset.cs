//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Room Loading/Room Load Options", fileName = "Room Load Options")]
    public class RoomLoadOptionsAsset : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool toInvestigation = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool blackTransition = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool fadeOutOnLoad = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int toWaypoint = -1;


        /// <summary>
        ///
        /// </summary>
        /// <param name="toWaypoint"></param>
        public virtual void SetWaypoint(int toWaypoint) => this.toWaypoint = toWaypoint;
    }
}