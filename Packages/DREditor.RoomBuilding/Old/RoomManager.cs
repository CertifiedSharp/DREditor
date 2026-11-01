//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.Progressing;
using DREditor.Progressing.Handling;
using DREditor.RoomBuilding.Implementation;
using DREditor.Services;
using UnityEngine;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    public class RoomManager : RoomManagerBase
    {
        public RoomBuilder Builder = null;

        public void SetBuilder(RoomBuilder Builder) => this.Builder = Builder;

        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            DialogueEventSystem.StartListening("LoadInvestigationRoom", LoadInvestigationInstance);
        }

        private void OnDisable()
        {
            DialogueEventSystem.StopListening("LoadInvestigationRoom", LoadInvestigationInstance);
        }

        /// <inheritdoc/>
        public override bool HasBase(int chapterNum, object pValue)
        {
            bool result = Builder.Sections[chapterNum].HasDataFor((Objective)pValue);

            UnityEngine.Debug.Log("Has Base for current room: " + result);

            return result;
        }

        /// <inheritdoc/>
        public override object SaveRoomState(object pValue)
        {
            RoomData roomI = NewSaveRoom((Objective)pValue);

            RoomData baseRoom = GetBaseData((Objective)pValue);

            if (baseRoom != null)
                roomI.SetPersistent(baseRoom.isPersistent);

            //onRoomSaved.SetValue(true);

            UnityEngine.Debug.Log(JsonUtility.ToJson(roomI));

            return roomI;
        }

        RoomData GetBaseData(Objective pValue)
        {
            RoomSection current = Builder.Sections.Find(n => n.Chapter == ServiceLocator.GetService<ProgressionManagerBase>().GetCurrentChapter());

            for (int i = 0; i < current.Rooms.Count; i++)
                if (current.Rooms[i].progressionValue.Description == pValue.Description)
                    return current.Rooms[i];

            return null;
        }

        public RoomData NewSaveRoom(Objective pValue)
        {
            RoomData room = new(pValue);

            room = Save(room);

            return room;
        }

        public RoomData OverwriteRoom(RoomData room)
        {
            room = Save(room);

            return room;
        }

        RoomData Save(RoomData room)
        {
            room.SetActorDatas(ActorLoader.Save());

            room.SetItemDatas(ItemActorLoader.SaveItems());

            room.Save();
            
            return room;
        }

        public void LoadRoom(RoomData room, RoomData instanceRoom = null)
        {
            if (room != null)
            {
                UnityEngine.Debug.Log("Loading Room Save: " + room.progressionValue.Description);

                SpawnableLoader.LoadSpawnables(room.spawnDatas);

                ActorLoader.Load(room, instanceRoom);

                ItemActorLoader.LoadItems(room.itemDatas, instanceRoom?.itemDatas);

                if (instanceRoom != null)
                {
                    TriggerLoader.Load(room.triggers, instanceRoom.triggers);

                    SubAreaLoader.Load(room.subAreas, instanceRoom.subAreas);

                    DoorLoader.Load(room.doorDatas, instanceRoom.doorDatas);

                    MoveLoader.Load(room.moveDatas, instanceRoom.moveDatas);

                    SaveableManager.Load(room.saveables);

                    SaveableManager.Load(instanceRoom.saveables);
                }
                else
                {
                    TriggerLoader.Load(room.triggers);

                    SubAreaLoader.Load(room.subAreas);

                    DoorLoader.Load(room.doorDatas);

                    MoveLoader.Load(room.moveDatas);

                    SaveableManager.Load(room.saveables);
                }
            }

        }

        /// <inheritdoc/>
        public override void LoadRoomState(object pValue, object data) // To Load Rooms in Game
        {
            RoomSection current = Builder.Sections
                .Find(n => n.Chapter == ServiceLocator.GetService<ProgressionManagerBase>().GetCurrentChapter());

            if (current == null)
            {
                UnityEngine.Debug.LogWarning("REPORT: ROOM BUILDER DID NOT HAVE A ROOM SECTION FOR THAT CHAPTER");

                current = Builder.Sections[0];
            }

            RoomData baseRoom = null;

            for (int i = 0; i < current.Rooms.Count; i++)
            {
                RoomData room = current.Rooms[i];

                if (room.progressionValue.Description == ((Objective)pValue).Description)
                {
                    baseRoom = room;

                    break;
                }
            }

            if (baseRoom == null)
                FindBaseRoomState(out baseRoom, pValue, data);

            if (baseRoom == null)
                SetBaseRoomToShell(out baseRoom, pValue);

            if (data != null && baseRoom != null) // Has Instance Data
                LoadRoom(baseRoom, (RoomData)data);
            else
                LoadRoom(baseRoom);

            onRoomInstanceLoaded.SetValue(true);
        }


        void FindBaseRoomState(out RoomData baseRoom, object pValue = null, object data = null)
        {
            RoomSection current = Builder.Sections
                .Find(n => n.Chapter == ServiceLocator.GetService<ProgressionManagerBase>().GetCurrentChapter());

            baseRoom = null;

            bool found = false;

            for (int x = 0; x < Builder.Sections.Count; x++)
            {
                current = Builder.Sections[x];

                for (int i = 0; i < current.Rooms.Count; i++)
                {
                    RoomData room = current.Rooms[i];

                    if (room.progressionValue.Description == ((Objective)pValue).Description ||
                        (data != null &&
                        room.progressionValue.Description == ((RoomData)data).progressionValue.Description))
                    {
                        found = true;

                        baseRoom = room;

                        break;
                    }
                }

                if (found)
                    break;
            }
        }


        void SetBaseRoomToShell(out RoomData baseRoom, object pValue = null)
        {
            baseRoom = null;

            UnityEngine.Debug.Log("Couldn't find Base Room data for Objective: " + ((Objective)pValue).Description +
                    " , Checking for Room Shell.");

            if (Builder.Shells.Count != 0)
                UnityEngine.Debug.Log("Shell Data has been found! Loading Shell...");

            if (Builder.Shells.Count != 0)
                baseRoom = Builder.Shells[0];
            else
                UnityEngine.Debug.Log("No Shell Data Has been Filled, Loading Empty Shell");
        }


        /// <inheritdoc/>
        public override bool BaseIsPersistent(object pValue)
        {
            return GetBaseData((Objective)pValue).isPersistent;
        }

        void LoadInvestigationInstance(object o = null)
        {
            ActorLoader.UnloadActors(false);

            TriggerLoader.UnloadEvents();

            LoadRoomState((Objective)o, null);
        }

        /// <inheritdoc/>
        public override void UnloadAll(bool inEditor)
        {
            ActorLoader.UnloadActors(false);

            TriggerLoader.UnloadEvents();
        }
    }
}