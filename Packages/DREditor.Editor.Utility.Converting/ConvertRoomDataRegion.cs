//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Doors;
using DREditor.Editor;
using DREditor.Progressing;
using DREditor.RoomBuilding;
using DREditor.RoomBuilding.Implementation;
using DREditor.RoomBuilding.Implementation.Actors;
using DREditor.RoomBuilding.Old;
using DREditor.SubAreas;
using DREditor.Talkables;
using DREditor.Triggers;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility.Converting
{
    /// <summary>
    ///
    /// </summary>
    public class ConvertRoomDataRegion
    {
        /// <summary>
        ///
        /// </summary>
        protected ProgressionDatabase pDB;

        /// <summary>
        ///
        /// </summary>
        protected RoomBuilder currentRoomBuilder;

        /// <summary>
        ///
        /// </summary>
        protected RoomSection currentRoomSection;

        /// <summary>
        ///
        /// </summary>
        protected RoomData currentRoomData;

        /// <summary>
        ///
        /// </summary>
        protected RoomStateBuilder currentRoomStateBuilder;

        /// <summary>
        ///
        /// </summary>
        protected RoomStateSection currentRoomStateSection;


        [MenuItem("DREditor/Convert/Convert First Room Data Builder")]
        public static void ConvertFirstRoomDataBuilders() => new ConvertRoomDataRegion().ConvertFirst();

        /// <summary>
        ///
        /// </summary>
        public virtual void ConvertFirst()
        {
            var roomBuilders = BuilderUtility.FindAssetsByType<RoomBuilder>();

            currentRoomBuilder = roomBuilders[0];

            CreateRoomStateBuilder(roomBuilders[0]);
        }


        [MenuItem("DREditor/Convert/Convert All Room Data Builders")]
        public static void ConvertAllRoomDataBuilders() => new ConvertRoomDataRegion().ConvertAll();

        /// <summary>
        ///
        /// </summary>
        public virtual void ConvertAll()
        {
            if (!DatabaseUtility.InitializeDatabase(pDB, out pDB))
                return;

            var roomBuilders = BuilderUtility.FindAssetsByType<RoomBuilder>();

            foreach (var roomBuilder in roomBuilders)
                CreateRoomStateBuilder(roomBuilder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomBuilder"></param>
        public virtual void CreateRoomStateBuilder(RoomBuilder roomBuilder)
        {
            currentRoomBuilder = roomBuilder;

            InitializeBuilder(roomBuilder.name.Replace(".asset", ""));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builderName"></param>
        protected virtual void InitializeBuilder(string builderName)
        {
            var builderAsset = ScriptableObject.CreateInstance<RoomStateBuilder>();

            string buildFolderPath = "Assets/RoomBuilding";

            if (!ConvertUtility.ProjectFolderExists(buildFolderPath))
                AssetDatabase.CreateFolder("Assets", "RoomBuilding");

            string buildFolder = AssetDatabase.CreateFolder(buildFolderPath, builderName);

            if (buildFolder == "")
            {
                UnityEngine.Debug.LogWarning("Could not create folder. Canceling Initialization.");

                UnityEngine.Debug.Log("Builder Path: " + buildFolderPath);

                return;
            }

            string newFolderPath = AssetDatabase.GUIDToAssetPath(buildFolder);

            AssetDatabase.CreateAsset(builderAsset, newFolderPath + "/" + builderName + ".asset");

            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(builderAsset);

            InitializeSections(builderAsset);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void InitializeSections(RoomStateBuilder builder)
        {
            foreach (Chapter chapter in pDB.Chapters)
            {
                var sectionAsset = ScriptableObject.CreateInstance<RoomStateSection>();

                sectionAsset.SetChapter(chapter);


                string newAssetPath = CreateSectionAssetPath(chapter, builder);

                if (newAssetPath == "")
                    return;


                AssetDatabase.CreateAsset(sectionAsset, newAssetPath);

                AssetDatabase.SaveAssets();

                EditorUtility.SetDirty(sectionAsset);

                builder.Sections.Add(sectionAsset);

                EditorUtility.SetDirty(builder);
            }

            currentRoomStateBuilder = builder;

            CreateDataAssets();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapter"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual string CreateSectionAssetPath(Chapter chapter, RoomStateBuilder builder)
        {
            string builderPath = AssetDatabase.GetAssetPath(builder);

            string builderPrefix = builder.name.Replace(".asset", "");

            string sectionPath = builderPath.Replace("/" + builder.name + ".asset", "");

            string sectionFolderName = builderPrefix + $"_{pDB.GetChapterIndex(chapter)}";

            string sectionFolder = AssetDatabase.CreateFolder(sectionPath, sectionFolderName);

            if (sectionFolder == "")
            {
                UnityEngine.Debug.LogWarning("Could not create folder. Canceling Initialization.");

                UnityEngine.Debug.Log("Builder Path: " + sectionPath);

                return "";
            }

            string newFolderPath = AssetDatabase.GUIDToAssetPath(sectionFolder);

            return newFolderPath + "/" + builderPrefix + $"_{pDB.GetChapterIndex(chapter)}.asset";
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateDataAssets()
        {
            foreach (var section in currentRoomBuilder.Sections)
            {
                currentRoomSection = section;

                currentRoomStateSection = currentRoomStateBuilder.Sections[currentRoomBuilder.Sections.IndexOf(section)];

                ConvertRoom();
            }

            for (int i = 0; i < currentRoomBuilder.Shells.Count; i++)
                CreateShellSave(i);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ConvertRoom()
        {
            for (int i = 0; i < currentRoomSection.Rooms.Count; i++)
            {
                currentRoomData = currentRoomSection.Rooms[i];

                SaveRoom(currentRoomData.progressionValue);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        protected virtual void SaveRoom(Objective pValue)
        {
            int index = pDB.Chapters.IndexOf(currentRoomStateSection.Chapter);

            string sectionPath = currentRoomStateBuilder.Sections[index].GetFolderPath();

            string sectionPrefix = currentRoomStateBuilder.Sections[index].name.Replace(".asset", "");

            string roomStateName = sectionPrefix + "_" + pValue.Description;

            if (!ConvertUtility.TryCreateFolder(ref sectionPath, roomStateName))
                return;

            string newAssetPath = sectionPath + "/" + roomStateName + ".asset";

            RoomStateData room = CreateRoomState(pValue, newAssetPath);

            room.SetPersistent(currentRoomData.isPersistent);

            currentRoomStateBuilder.Sections[index].Rooms.Add(room);

            string path = AssetDatabase.GetAssetPath(room);

            if (path.Length >= 252)
                UnityEngine.Debug.LogWarning("path too long for: " + path);

            EditorUtility.SetDirty(currentRoomStateBuilder);

            EditorUtility.SetDirty(currentRoomStateBuilder.Sections[index]);

            EditorUtility.SetDirty(room);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual RoomStateData CreateRoomState(Objective pValue, string path, RoomStateData data = null)
        {
            bool createAsset = false;

            if (data == null)
            {
                data = ScriptableObject.CreateInstance<RoomStateData>();

                data.SetProgressionValue(pValue);


                createAsset = true;
            }

            string dataAssetPath = path.Replace(".asset", "");

            data = ConvertRoomData(data, dataAssetPath);

            if (createAsset)
                ConvertUtility.CreateAssetAt(data, path);

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual RoomStateData ConvertRoomData(RoomStateData data, string path)
        {
            data.dataStates.Add(SaveDataAsset<SpawnableStateData, GameObject>(path, "Spawnables", currentRoomData.spawnDatas));

            data.dataStates.Add(SaveDataAsset<ActorStateData, ActorData>(path, "Actors", currentRoomData.actorDatas));

            data.dataStates.Add(SaveDataAsset<ItemStateData, ItemData>(path, "Items", currentRoomData.itemDatas));

            data.dataStates.Add(SaveDataAsset<TriggerStateData, EventTrigger>(path, "Triggers", currentRoomData.triggers));

            data.dataStates.Add(SaveDataAsset<SubAreaStateData, SubAreaData>(path, "SubAreas", currentRoomData.subAreas));

            data.dataStates.Add(SaveDataAsset<DoorStateData, DoorData>(path, "Doors", currentRoomData.doorDatas));

            data.dataStates.Add(SaveDataAsset<MoveStateData, MoveData>(path, "Moveables", currentRoomData.moveDatas));

            data.dataStates
                .Add(SaveDataAsset<SaveableStateData, SaveableDataSet>(path, "Saveables", currentRoomData.saveables.dataSets));

            data.dataStates.RemoveAll(item => item == null);

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="path"></param>
        /// <param name="dataAssetName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual ScriptableObject SaveDataAsset<T, T2>(string path, string dataAssetName, List<T2> list)
            where T : StateDataAsset<T2>
        {
            if (list.Count == 0)
                return null;

            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", 
                new[] { Path.GetDirectoryName(path.TrimEnd(Path.DirectorySeparatorChar)) });

            T dataAsset;

            if (guids.Length > 0)
                dataAsset = ConvertUtility.GetAssetFromGUID<T>(guids[0]);
            else
                dataAsset = ScriptableObject.CreateInstance<T>();

            dataAsset.SetDatas(list);

            if (!(guids.Length > 0) && dataAsset.datas != null && dataAsset.datas.Count != 0)
                AssetDatabase.CreateAsset(dataAsset, path + "_" + dataAssetName + ".asset");

            EditorUtility.SetDirty(dataAsset);

            AssetDatabase.SaveAssets();

            if (guids.Length > 0)
                return null;

            return dataAsset;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void CreateShellSave(int i)
        {
            if (!CreateShellPath($"_{i}_Shell", out string newAssetPath))
            {
                UnityEngine.Debug.LogWarning("ALERT: Had trouble creating shell path, aborting for index: " + i);

                return;
            }

            currentRoomData = currentRoomBuilder.Shells[i];

            RoomStateData shellAsset = CreateRoomState(null, newAssetPath);

            CheckFillShells(i);

            if (currentRoomStateBuilder.Shells.HasValidIndexFor(i))
                currentRoomStateBuilder.Shells[i] = shellAsset;
            else
                currentRoomStateBuilder.Shells.Add(shellAsset);

            EditorUtility.SetDirty(currentRoomStateBuilder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builderSuffix"></param>
        /// <param name="newAssetPath"></param>
        /// <returns></returns>
        protected virtual bool CreateShellPath(string builderSuffix, out string newAssetPath)
        {
            newAssetPath = "";

            string shellPath = currentRoomStateBuilder.GetFolderPath();

            if (!ConvertUtility.TryCreateFolder(ref shellPath, currentRoomStateBuilder.name + "_Shell"))
                return false;

            string builderPrefix = currentRoomStateBuilder.name.Replace(".asset", "");

            string shellAssetName = builderPrefix + builderSuffix;

            if (!ConvertUtility.TryCreateFolder(ref shellPath, shellAssetName))
                return false;

            newAssetPath = shellPath + "/" + shellAssetName + ".asset";

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void CheckFillShells(int i)
        {
            if (currentRoomStateBuilder.Shells.Count < i)
            {
                currentRoomStateBuilder.Shells.Add(null);

                CheckFillShells(i);
            }
        }
    }
}