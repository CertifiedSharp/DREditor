//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using System;
using UnityEngine;

namespace DREditor.RoomBuilding.Instances
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RoomInstanceData
    {
        /// <summary>
        ///
        /// </summary>
        public string Name = "";

        /// <summary>
        ///
        /// </summary>
        public bool isPersistent = false;

        /// <summary>
        ///
        /// </summary>
        public Objective progressionValue;

        /// <summary>
        ///
        /// </summary>
        [TextArea]
        public string roomData = "";


        public RoomInstanceData() { }

        public RoomInstanceData(string Name, bool isPersistent, Objective progressionValue, string roomData)
        {
            this.Name = Name;

            this.isPersistent = isPersistent;

            this.progressionValue = progressionValue;

            this.roomData = roomData;
        }

        public RoomInstanceData(string Name, Objective progressionValue)
        {
            this.Name = Name;

            this.progressionValue = progressionValue;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="isPersistent"></param>
        public virtual void SetPersistent(bool isPersistent) => this.isPersistent = isPersistent;

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomData"></param>
        public virtual void SetRoomData(string roomData) => this.roomData = roomData;
    }
}