//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class Room
    {
        public string Name;

        public RoomData Data;

        public Room() { }

        public Room(string Name, RoomData Data)
        {
            this.Name = Name;

            this.Data = Data;
        }
    }
}