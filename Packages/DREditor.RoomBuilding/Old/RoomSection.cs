//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using System;
using System.Collections.Generic;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RoomSection
    {
        public Chapter Chapter;

        public List<RoomData> Rooms = new();

        public bool HasDataFor(Objective pValue)
        {
            foreach (RoomData r in Rooms)
                if (pValue.Description == r.progressionValue.Description)
                    return true;

            return false;
        }

        public RoomData GetDataWithObjective(Objective pValue)
        {
            foreach (RoomData r in Rooms)
                if (pValue.Description == r.progressionValue.Description)
                    return r;

            return null;
        }
    }
}