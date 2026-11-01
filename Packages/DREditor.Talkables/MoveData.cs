//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class MoveData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string name;

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


        public MoveData() { }

        public MoveData(string name, Vector3 position, Vector3 rotation)
        {
            this.name = name;

            this.position = position;

            this.rotation = rotation;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone() => MemberwiseClone();
    }
}