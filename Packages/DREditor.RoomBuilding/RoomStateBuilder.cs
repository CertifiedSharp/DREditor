//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Room Building/Room State Builder", fileName = "Room State Builder", order = 0)]
    public class RoomStateBuilder : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space(10)]
        [Header("")]
        [Divider]
        [FormerlySerializedAs("Shell")]
        [Tooltip("")]
        public List<RoomStateData> Shells = new();

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<RoomStateSection> Sections = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="objective"></param>
        /// <returns>
        ///
        /// </returns>
        public RoomStateSection GetSectionWithObjective(Chapter chapter, Objective objective)
        {
            foreach (RoomStateSection section in Sections)
                if (section.Chapter == chapter && section.HasDataFor(objective.Description))
                    return section;

            return null;
        }
    }
}