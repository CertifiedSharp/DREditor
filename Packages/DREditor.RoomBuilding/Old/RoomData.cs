//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Doors;
using DREditor.Progressing;
using DREditor.RoomBuilding.Implementation;
using DREditor.RoomBuilding.Implementation.Actors;
using DREditor.SubAreas;
using DREditor.Talkables;
using DREditor.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RoomData
    {
        public bool isPersistent = false; // At present this only persists properly if it's the current room of InstanceData

        public Objective progressionValue;

        [FormerlySerializedAs("spawnList")]
        public List<GameObject> spawnDatas = new ();

        [FormerlySerializedAs("actorList")]
        public List<ActorData> actorDatas = new ();

        [FormerlySerializedAs("itemList")]
        public List<ItemData> itemDatas = new ();

        public List<EventTrigger> triggers = new();

        public List<SubAreaData> subAreas = new();

        public List<DoorData> doorDatas = new ();

        public List<MoveData> moveDatas = new();

        public SaveableData saveables = new ();


        public RoomData() { }

        public RoomData(Objective progressionValue)
        {
            this.progressionValue = progressionValue;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                spawnDatas = SpawnableLoader.SaveSpawnables();
#endif
        }

        public void SetPersistent(bool isPersistent) => this.isPersistent = isPersistent;


        public void SetActorDatas(List<ActorData> actorDatas) => this.actorDatas = actorDatas;
        public void SetItemDatas(List<ItemData> itemDatas) => this.itemDatas = itemDatas;


        public void Save()
        {
#if UNITY_EDITOR
            spawnDatas = SpawnableLoader.SaveSpawnables();
#endif
            

            triggers = TriggerLoader.SaveData();

            subAreas = SubAreaLoader.SaveData();

            doorDatas = DoorLoader.SaveData();

            moveDatas = MoveLoader.SaveData();

            saveables = SaveableManager.SaveData();
        }
    }
    /* - List the Objects you want to save in the RoomData class
     * - Below is usually done with a static Class to call Save() and Load()
     * - Go to RoomManager.cs and fill out the save process in the NewSaveRoom function
     * - Go to RoomManager.cs and fill out the load process in the LoadRoom function
     * - Go to RoomManager.cs and fill out the load process in the LoadRoomInstance function
     * - Go to RoomManager.cs and fill out the overwrite process in the Overwrite function
     * - Make sure you make the Save and Load functions on the class you're saving and loading to
     */
}