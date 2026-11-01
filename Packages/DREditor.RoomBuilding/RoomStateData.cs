//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RoomStateData : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space(10)]
        [Header("")]
        [Divider]
        [Tooltip("")]
        public bool isPersistent = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Objective progressionValue;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<ScriptableObject> dataStates = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="progressionValue"></param>
        public virtual void SetProgressionValue(Objective progressionValue) => this.progressionValue = progressionValue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="isPersistent"></param>
        public virtual void SetPersistent(bool isPersistent) => this.isPersistent = isPersistent;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string GetProgressionDescription() => progressionValue.Description;
    }
}