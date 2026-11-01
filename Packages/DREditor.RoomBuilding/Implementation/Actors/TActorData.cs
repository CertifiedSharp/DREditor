//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.RoomBuilding.Implementation.Actors
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [Serializable]
    public class TActorData : ActorData
    {
        public TActorData() { }

        public TActorData(string refName, GameObject actor, bool ident, string spriteName, Material shared)
        {
            position = actor.transform.position;

            rotation = actor.transform.eulerAngles;

            aReference = refName;

            mat = shared;

            identified = ident;

            matName = spriteName;
        }

        /// <inheritdoc/>
        public override object Clone() => base.Clone() as TActorData;
    }
}