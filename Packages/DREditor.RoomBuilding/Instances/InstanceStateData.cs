//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using System;
using System.Collections.Generic;

namespace DREditor.RoomBuilding.Instances
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class InstanceStateData
    {
        /// <summary>
        ///
        /// </summary>
        public Objective CurrentObjective;

        /// <summary>
        ///
        /// </summary>
        public RoomInstanceData CurrentRoom;

        /// <summary>
        ///
        /// </summary>
        public List<RoomInstanceData> Rooms = new();


        public InstanceStateData() { }

        public InstanceStateData(Objective CurrentObjective, RoomInstanceData CurrentRoom)
        {
            this.CurrentObjective = CurrentObjective;

            this.CurrentRoom = CurrentRoom;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="CurrentObjective"></param>
        public virtual void SetCurrentObjective(Objective CurrentObjective) => this.CurrentObjective = CurrentObjective;

        /// <summary>
        ///
        /// </summary>
        /// <param name="CurrentRoom"></param>
        public virtual void SetCurrentRoom(RoomInstanceData CurrentRoom) => this.CurrentRoom = CurrentRoom;
    }
}