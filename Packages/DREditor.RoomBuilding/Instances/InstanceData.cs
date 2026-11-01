//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using DREditor.RoomBuilding.Old;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomBuilding.Instances
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class InstanceData
    {
        public Objective CurrentObjective;

        public List<Room> Rooms = new ();

        public Room CurrentRoom;

        public Vector3 Position;

        public Vector3 Rotation;

        public Vector3 MainPosition;

        public Vector3 MainRotation;

        public Vector3 DialoguePosition;

        public Vector3 DialogueRotation;

        public Vector3 BlurPosition;

        public Vector3 BlurRotation;

        public Vector2 MouseAbs;

        public RoomInstanceData currentDynamicRoom;

        public List<RoomInstanceData> dynamicRooms = new();
    }
}