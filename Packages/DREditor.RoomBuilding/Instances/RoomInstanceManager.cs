//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Progressing;
using DREditor.Progressing.Handling;
using DREditor.RoomBuilding.Old;
using DREditor.RoomLoading;
using DREditor.Saving;
using DREditor.Services;
using DREditor.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.RoomBuilding.Instances
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(InstanceManagerBase) + ", " + nameof(IDRESaveable) + ", " + nameof(IDRELoadable) + ", " + 
        nameof(IDRELoadableManager) + ", " + nameof(IRoomLoadRoutine) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class RoomInstanceManager : InstanceManagerBase, IDRESaveable, IDRELoadable, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent objectiveKeepsInstanceData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveKey = "RoomData";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKeyObjective = "CurrentObjective";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InstanceStateData data = null;


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas = new();


        #region Save Room Instance

        /// <inheritdoc/>
        public override void SaveRoomInstance(string roomName)
        {
            if (data.CurrentObjective.Description == "")
            {
                CreateNewInstanceData();

                SetCurrentObjective();
            }

            int chapterNumber = currentChapterIndex.Value;

            bool hasData = ContainsRoomDataFor(roomName);

            bool hasBaseData = ServiceLocator.GetService<RoomManagerBase>().HasBase(chapterNumber, data.CurrentObjective);

            if (hasBaseData || hasData)
                if (hasData)
                    ReplaceExistingInstanceData(roomName);
                else
                    AddNewInstanceData(roomName);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateNewInstanceData()
        {
            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": New Instance Data Overriden/ Created");

            data = new();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetCurrentObjective()
        {
            data.SetCurrentObjective(ServiceLocator.GetService<ProgressionManagerBase>().currentObjective);

            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": Load File Objective is: " + data.CurrentObjective.Description);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        protected virtual void ReplaceExistingInstanceData(string roomName)
        {
            Objective objective;

            objective = data.Rooms[GetIndexOfRoom(roomName)].progressionValue;

            object instance = ServiceLocator.GetService<RoomManagerBase>().SaveRoomState(objective);

            //if (instance is RoomData roomData)
            //    data.Rooms[GetIndexOfRoom(roomName)].Data = roomData;

            if (instance is string roomWrapper)
                data.Rooms[GetIndexOfRoom(roomName)].SetRoomData(roomWrapper);

            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": Data of this Room Instance Replaced");
        }

        /// <inheritdoc/>
        protected override int GetIndexOfRoom(string roomName)
        {
            for (int i = 0; i < data.Rooms.Count; i++)
                if (data.Rooms[i].Name == roomName)
                    return i;

            return -1; // Shouldn't reach here
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        protected virtual void AddNewInstanceData(string roomName)
        {
            object instance = ServiceLocator.GetService<RoomManagerBase>().SaveRoomState(data.CurrentObjective);

            bool isPersistent = ServiceLocator.GetService<RoomManagerBase>().BaseIsPersistent(data.CurrentObjective);

            if (instance is RoomData roomData)
            {
                Room room = new(roomName, roomData);

                room.Data.SetPersistent(isPersistent);

                data.Rooms.Add(ConvertInstanceData.ConvertRoom(room));

                UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": New data for this Room Instance Saved: " + roomName);
            }

            if (instance is string roomWrapper)
            {
                RoomInstanceData roomInstanceData = new(roomName, isPersistent, data.CurrentObjective, roomWrapper);

                data.Rooms.Add(roomInstanceData);

                UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": New data for this Room Instance Saved: " + roomName);
            }
        }

        #endregion

        #region Load Room Instance

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => this.saveDatas = saveDatas;

        /// <inheritdoc/>
        public override void LoadRoomInstance(string roomName)
        {
            EnsureDataCanBeLoaded();

            var baseRoomManager = ServiceLocator.GetService<RoomManagerBase>();

            if (isLoadingFile.Value && saveDatas != null && data.CurrentRoom.Name == roomName)
                LoadSaveFileInstanceRoom(roomName);
            else if (ContainsRoomDataFor(roomName) && data != null)
                OnLoadExistingInstanceData(roomName);
            else
                baseRoomManager.LoadRoomState(data.CurrentObjective, null);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnsureDataCanBeLoaded()
        {
            var progressionManager = ServiceLocator.GetService<ProgressionManagerBase>();

            bool currentObjectiveInvalid = data.CurrentObjective.Description == "";

            bool objectivesDontMatch = data.CurrentObjective != progressionManager.currentObjective;

            bool objectiveIsInvalid = currentObjectiveInvalid || objectivesDontMatch;

            bool dataCannotBeLoaded = !isLoadingFile.Value && objectiveIsInvalid;

            if (dataCannotBeLoaded)
            {
                if (data == null || !objectiveKeepsInstanceData.Value)
                    if (data != null)
                        if (data.CurrentRoom.isPersistent)
                            OnCurrentRoomPersistent();
                        else
                            CreateNewInstanceData();
                    else
                        CreateNewInstanceData();

                SetCurrentObjective();

                UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": Ensured Data could be loaded");
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCurrentRoomPersistent()
        {
            data.CurrentRoom.SetPersistent(false);

            RoomInstanceData currentRoom = data.CurrentRoom;

            CreateNewInstanceData();

            data.Rooms.Add(currentRoom);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        protected virtual void LoadSaveFileInstanceRoom(string roomName)
        {
            loadProgressionData.Raise();

            SetCurrentObjective();

            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": Loading CurrentRoom Data from save file.");

            var objective = saveDatas.Get<Objective>(saveKeyObjective);

            var baseRoomManager = ServiceLocator.GetService<RoomManagerBase>();

            var dynamicRooms = data.Rooms.Where(roomInstanceData => roomInstanceData.Name == roomName).ToList();

            if (data.CurrentRoom.Name == roomName)
                baseRoomManager.LoadRoomState(objective, data.CurrentRoom.roomData);
            else if(dynamicRooms.Count != 0)
                baseRoomManager.LoadRoomState(objective, dynamicRooms[0].roomData);
            else
                baseRoomManager.LoadRoomState(objective, data.CurrentRoom.roomData);

            //UseConversion();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomName"></param>
        protected virtual void OnLoadExistingInstanceData(string roomName)
        {
            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ":Has Room Instance for: " + roomName);

            var baseRoomManager = ServiceLocator.GetService<RoomManagerBase>();

            var dynamicRooms = data.Rooms.Where(roomInstanceData => roomInstanceData.Name == roomName).ToList();

            if (dynamicRooms.Count != 0)
                baseRoomManager.LoadRoomState(data.CurrentObjective, dynamicRooms[0].roomData);
            else
                baseRoomManager.LoadRoomState(data.CurrentObjective, data.Rooms[GetIndexOfRoom(roomName)].roomData);
        }

        //void UseConversion()
        //{
        //    if (baseRoomManager is RoomStateManager)
        //    {
        //        var dynamicRooms = data.dynamicRooms.Where(x => x.roomName == roomName).ToList();

        //        if (dynamicRooms.Count != 0)
        //            baseRoomManager.LoadRoomInstance(currentObjective, dynamicRooms[0].roomData);
        //        else
        //            LoadSaveFileBaseDataConvert(currentObjective);
        //    }
        //    else
        //        baseRoomManager.LoadRoomInstance(currentObjective, data.CurrentRoom.Data);
        //}

        //// So when we load a save file we take the old RoomData and turn it into the new dynamic data.
        //void LoadSaveFileBaseDataConvert(Objective currentObjective)
        //{
        //    RoomData roomData = data.CurrentRoom.Data;

        //    List<object> test = new()
        //    {
        //        JsonUtility.ToJson(roomData.actorList),
        //        JsonUtility.ToJson(roomData.itemList),
        //        JsonUtility.ToJson(roomData.triggers),
        //        JsonUtility.ToJson(roomData.subAreas),
        //        JsonUtility.ToJson(roomData.doorDatas),
        //        JsonUtility.ToJson(roomData.moveDatas),
        //        JsonUtility.ToJson(roomData.saveables)
        //    };

        //    string testData = JsonUtility.ToJson(test);

        //    ServiceLocator.GetService<RoomManagerBase>().LoadRoomInstance(currentObjective, testData);
        //}

        #endregion

        #region Clear Data

        /// <inheritdoc/>
        public override void ClearRoomData()
        {
            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": Cleared Room List on Instance Data.");

            data.Rooms.Clear();
        }

        /// <inheritdoc/>
        public override void ClearData()
        {
            UnityEngine.Debug.Log(nameof(RoomInstanceManager) + ": Instance Manager data class cleared");

            CreateNewInstanceData();
        }

        #endregion

        #region Save Game File

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            data.SetCurrentObjective(ServiceLocator.GetService<ProgressionManagerBase>().currentObjective);

            RoomInstanceData CurrentRoom = new(SceneManager.GetActiveScene().name, data.CurrentObjective);

            if (!ServiceLocator.HasService(out RoomManagerBase baseRoomManager))
                return;

            CurrentRoom.SetPersistent(baseRoomManager.BaseIsPersistent(data.CurrentObjective));

            try
            {
                if (!inTrialMode.Value)
                {
                    object currentRoomSaveData = baseRoomManager.SaveRoomState(data.CurrentObjective);

                    if (currentRoomSaveData is RoomData currentRoomData)
                        CurrentRoom.SetRoomData(ConvertInstanceData.ConvertInstanceRoomDatas(currentRoomData));

                    if (currentRoomSaveData is string roomData)
                        CurrentRoom.SetRoomData(roomData);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning("REPORT: The Room Instance data was not saved, this is usually due to " +
                    "a RoomManager Instance not being available." +
                    " The RoomManager Instance is: " + baseRoomManager != null + "\n Error: " + e.ToString());
            }

            data.SetCurrentRoom(CurrentRoom);

            UnityEngine.Debug.Log(nameof(RoomInstanceManager) +
                ": Current Room Progress Saved at Objective: " + data.CurrentObjective.Description);

            saveData.Set(saveKey, data);
        }


        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            try
            {
                InstanceData loadedInstanceData = saveDatas.Get<InstanceData>(saveKey);

                data = ConvertInstanceData.Convert(loadedInstanceData);
            }
            catch
            {
                try
                {
                    UnityEngine.Debug.Log("REPORT: Old Instance State Data not found, loading normally...");

                    InstanceStateData loadedInstanceData = saveDatas.Get<InstanceStateData>(saveKey);

                    data = loadedInstanceData.DeepCopy();

                    UnityEngine.Debug.Log("REPORT: Instance State Data Successfully Loaded.");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("ALERT: Loading Instance State Data Critically Failed." +
                        $"\nREASON: {ex}");
                }
            }

            if (data != null && data.CurrentRoom != null)
                UnityEngine.Debug.Log($"REPORT: {nameof(RoomInstanceManager)}: Will load to Scene: " + data.CurrentRoom.Name);
        }

        /// <inheritdoc/>
        public override IEnumerator OnRoomLoadRoutine()
        {
            if (!ServiceLocator.HasService<RoomManagerBase>())
                UnityEngine.Debug.Log($"REPORT: {nameof(RoomInstanceManager)}: The game has moved to a scene that does not contain " +
                    $"a room manager.");

            if (!inTrialMode.Value && ServiceLocator.HasService<RoomManagerBase>())
                yield return LoadRoomInstance();
            else if (isLoadingFile.Value)
                loadProgressionData.Raise();

            yield break;
        }

        #endregion
    }
}