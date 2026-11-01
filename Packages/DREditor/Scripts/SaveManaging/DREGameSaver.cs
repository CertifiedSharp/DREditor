//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.SaveManaging
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class DREGameSaver : GameSaver
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SerializableDictionary loadedDictionary = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string loadGameFileEventString = "LoadGameFile";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string applyDataEventString = "ApplyCurrentData";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveFileFoundEvent = "OnSaveFileFound";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string requestSaveFileEvent = "RequestSaveFile";


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            onLoadingRoom.Response.AddListener(LoadSaveData);

            DialogueEventSystem.StartListening(applyDataEventString, ApplyCurrentData);

            DialogueEventSystem.StartListening("SaveGameFile", SaveGameFile);

            DialogueEventSystem.StartListening(loadGameFileEventString, LoadGameFile);

            DialogueEventSystem.StartListening("LoadGameFromData", UpdateGameFromData);

            DialogueEventSystem.StartListening(requestSaveFileEvent, OnGetSaveFile);

            //Currently unused but could still be kept available.
            DialogueEventSystem.StartListening("OnRequestedSaveData", OnRequestedSaveData);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            onLoadingRoom.Response.RemoveListener(LoadSaveData);

            DialogueEventSystem.StopListening(applyDataEventString, ApplyCurrentData);

            DialogueEventSystem.StopListening("SaveGameFile", SaveGameFile);

            DialogueEventSystem.StopListening(loadGameFileEventString, LoadGameFile);

            DialogueEventSystem.StopListening("LoadGameFromData", UpdateGameFromData);

            DialogueEventSystem.StopListening(requestSaveFileEvent, OnGetSaveFile);

            //Currently unused but could still be kept available.
            DialogueEventSystem.StopListening("OnRequestedSaveData", OnRequestedSaveData);
        }

        /// <inheritdoc/>
        public override void SaveGameFile(object o = null)
        {
            string slotNum = (string)o;

            DRESaveData saveData = new();

            List<IDRESaveable> saveables = FindObjectsByType<MonoBehaviour>().OfType<IDRESaveable>().ToList();

            foreach (IDRESaveable d in saveables)
                d?.Save(saveData.saveDataEntries);

            SaveSystem.SaveToJSON(new SerializableDictionary(saveData.saveDataEntries), slotNum);

            CurrentDRESaveData = GetData(slotNum);
        }

        /// <inheritdoc/>
        public override DRESaveData GetData(string slotNum)
        {
            try
            {
                SerializableDictionary data = SaveSystem.ReadFromJSON<SerializableDictionary>(slotNum);

                Dictionary<string, object> dict = data.ToDictionary();

                if (dict.Keys.Count > 0)
                {
                    DRESaveData saveData = new(dict);

                    loadedDictionary = data;

                    return saveData;
                }
                else
                    return OnDifferentSaveStructure(slotNum);

            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);

                return OnDifferentSaveStructure(slotNum);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="slotNum"></param>
        /// <returns></returns>
        protected virtual DRESaveData OnDifferentSaveStructure(string slotNum) => new();


        /// <inheritdoc/>
        public override void ApplyCurrentData(object o = null)
        {
            o ??= false;

            List<IDRELoadableManager> loadables = FindObjectsByType<MonoBehaviour>().OfType<IDRELoadableManager>().ToList();

            foreach (IDRELoadableManager manager in loadables)
                manager?.LoadManager(CurrentDRESaveData.saveDataEntries, (bool)o);
        }
        

        /// <inheritdoc/>
        public override void UpdateGameFromData(object o = null)
        {
            Dictionary<string, object> newSaveData = ((SerializableDictionary)o).ToDictionary();

            if (CurrentDRESaveData != null)
            {
                List<ILoadOnChapterSelect> loadables = FindObjectsByType<MonoBehaviour>().OfType<ILoadOnChapterSelect>().ToList();

                foreach (ILoadOnChapterSelect manager in loadables)
                    manager?.CarryOverOnChapterSelect(CurrentDRESaveData.saveDataEntries, newSaveData);
            }

            CurrentDRESaveData = new DRESaveData(newSaveData);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadSaveData()
        {
            if (!isLoadingFile.Value)
                return;

            var loadables = FindObjectsByType<MonoBehaviour>().OfType<IDRELoadable>().ToList();

            foreach (var loadable in loadables)
                loadable.Load(CurrentDRESaveData.saveDataEntries);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void OnGetSaveFile(object o = null)
        {
            DRESaveData sentData = GetData((string)o);
            
            DialogueEventSystem.TriggerEvent(saveFileFoundEvent, sentData.saveDataEntries);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <remarks></remarks>
        protected virtual void OnRequestedSaveData(object o = null) 
            => DialogueEventSystem.TriggerEvent("SendRequestedSaveData", CurrentDRESaveData.saveDataEntries);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>UTILITY TO HELP CONVERT PASTED OLD CHAPTER SELECT SERIALIZED ENTRIES INTO A SAVE FILE AND THEN BACK INTO BETTER DATA.</remarks>
        [ContextMenu("Save Loaded Dictionary To Save File Thirty")]
        private void SaveLoadedDictionaryToSaveFileThirty() => SaveSystem.SaveToJSON(loadedDictionary, "save29");
    }
}

//void OldSaveData()
//{
//    // Make a new GameData
//    GameData data = new()
//    {
//        // Call from managers to assign the data for the games data
//        BaseData = new BaseSave() { SceneName = SceneManager.GetActiveScene().name },

//        MainData = DifficultyHandler.instance.Save(),

//        PlayerData = PlayerInfo.instance.Save(),

//        // Keep in mind that the CurrentObjective is actually stored in the room data
//        RoomData = ServiceLocator.GetService<InstanceManagerBase>().SaveRoomProgress(),


//        ProgressionData = ServiceLocator.GetService<ProgressionManager>().Save(),


//        DialogueData = new DialogueData()
//    };

//    data.MainData.location = currentLocation.Value;

//    FindAnyObjectByType<ModeHandler>().Save(data.MainData);
//    FindAnyObjectByType<StateHandler>().Save(data.MainData);


//    if (inTrialMode.Value && inDialogue.Value)
//    {
//        data.TrialData = new()
//        {
//            actorDatas = RoomManager.SaveTrialActors()
//        };

//        //OnSaveTrialDialogue?.Invoke(data);
//        // below replaces above for saving.
//        ServiceLocator.GetService<DialogueManager>()
//        .GetComponentsInChildren<MonoBehaviour>()
//        .OfType<IDialogueSavingExtension>()
//        .ToList()
//        .ForEach(saver => saver.SaveDialogue(data.DialogueData));
//    }
//    else
//    {
//        //OnSaveDialogue?.Invoke(data);
//        ServiceLocator.GetService<DialogueManager>()
//        .GetComponentsInChildren<MonoBehaviour>()
//        .OfType<IDialogueSavingExtension>()
//        .ToList()
//        .ForEach(saver => saver.SaveDialogue(data.DialogueData));
//    }
//    data.BackData = (DREditor.Dialogues.BacklogData)ServiceLocator.GetService<BacklogHandler>().Save();

//    data.MusicData = ServiceLocator.GetService<SoundManagerBase>().SaveMusic();

//    data.EnvData = ServiceLocator.GetService<SoundManagerBase>().SaveEnv();

//    data.ManagerData = ServiceLocator.GetService<SaveManagersHandler>().CreateManagerData();

//    CurrentDRESaveData = data;
//}


//void OldApplyCurrentData()
//{
//    DifficultyHandler.instance.Load(CurrentDRESaveData.Get<MainData>("MainData"));

//    FindAnyObjectByType<ModeHandler>().Load(CurrentDRESaveData.Get<MainData>("MainData"));

//    FindAnyObjectByType<StateHandler>().Load(CurrentDRESaveData.Get<MainData>("MainData"));

//    PlayerInfo.instance.Load(CurrentDRESaveData.Get<PlayerInfo.Data>("PlayerData"));

//    OptionsMenu.CallUpdateSettings();

//    ServiceLocator.GetService<InstanceManagerBase>().SetCurrentInstanceData(CurrentDRESaveData.Get<InstanceData>("RoomData"));

//    ServiceLocator.GetService<SaveManagersHandler>().LoadManagerData(CurrentDRESaveData.Get<ManagerData>("ManagerData"));

//    ServiceLocator.GetService<DialogueManager>().
//        GetManagerComponent<DialogueLoadIdentifier>()
//        .SetSaveDialogueIdentification(CurrentDRESaveData.Get<string>("DialogueName"));
//}