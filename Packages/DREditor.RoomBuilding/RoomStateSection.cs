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
    public class RoomStateSection : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space(10)]
        [Header("")]
        [Divider]
        [Tooltip("")]
        public Chapter Chapter;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        public List<RoomStateData> Rooms = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="Chapter"></param>
        public virtual void SetChapter(Chapter Chapter) => this.Chapter = Chapter;

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public virtual bool HasDataFor(object pValue)
        {
            foreach (RoomStateData roomStateData in Rooms)
                if (pValue is string progressionValue &&
                    progressionValue == roomStateData.GetProgressionDescription() ||
                    ((Objective)pValue).Description == roomStateData.GetProgressionDescription())
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public virtual object GetDataWithObjective(Objective pValue)
        {
            foreach (RoomStateData roomStateData in Rooms)
                if (pValue.Description == roomStateData.GetProgressionDescription())
                    return roomStateData;

            return null;
        }
    }
}