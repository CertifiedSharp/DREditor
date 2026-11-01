//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.RoomBuilding.Implementation;
using DREditor.Editor.Utility;
using UnityEngine;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    public class RoomManagerDisplayEditor : RoomSaveDisplayEditorBase
    {
        readonly RoomManager manager;

        public RoomManagerDisplayEditor(RoomManager manager)
        {
            this.manager = manager;
        }

        /// <inheritdoc/>
        public override void LoadRoom(object room)
        {
            ActorLoader.UnloadActors(true);

            manager.LoadRoom((RoomData)room);
        }

        /// <inheritdoc/>
        public override void OverwriteRoom(object current, object room, int i)
        {
            ((RoomSection)current).Rooms[i] = manager.OverwriteRoom((RoomData)room);
        }

        /// <inheritdoc/>
        public override void DeleteRoom(object current, object room) => ((RoomSection)current).Rooms.Remove((RoomData)room);

        /// <inheritdoc/>
        public override void DisplayRoomPieces(object roomData)
        {
            RoomData room = (RoomData)roomData;

            room.SetPersistent(HandyFields.BoolField(room.isPersistent, "Is Persistent: "));

            if (Button("Load Actors"))
                ActorLoader.Load(room);

            if (Button("Load Items"))
                ItemActorLoader.LoadItems(room.itemDatas);

            if (Button("Load Doors"))
                DoorLoader.Load(room.doorDatas);

            if (Button("Load Moveables"))
                MoveLoader.Load(room.moveDatas);

            if (Button("Load Triggers"))
                TriggerLoader.Load(room.triggers);

            if (Button("Load Spawnables"))
                SpawnableLoader.LoadSpawnables(room.spawnDatas);

            if (Button("Load SubAreas"))
                SubAreaLoader.Load(room.subAreas);

            if (Button("Load Saveables"))
                SaveableManager.Load(room.saveables);
        }

        protected bool Button(string s) => GUILayout.Button(s, GUILayout.Width(150));
    }
}
#endif