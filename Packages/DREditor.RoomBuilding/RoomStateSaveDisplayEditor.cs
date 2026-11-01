//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Progressing;
using DREditor.RoomStateBuilding;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public class RoomStateSaveDisplayEditor : RoomSaveDisplayEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        readonly RoomStateManager manager;

        /// <summary>
        ///
        /// </summary>
        readonly RoomStateBuilder builder;


        public RoomStateSaveDisplayEditor(RoomStateManager manager, RoomStateBuilder builder)
        {
            this.manager = manager;

            this.builder = builder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual RoomStateData CreateRoomState(Objective pValue, string path, RoomStateData data = null)
        {
            bool createAsset = false;

            if (data == null)
            {
                data = ScriptableObject.CreateInstance<RoomStateData>();

                data.SetProgressionValue(pValue);

                createAsset = true;
            }

            string dataAssetPath = path.Replace(".asset", "");

            UnityEngine.Debug.Log(path);

            foreach (var saver in manager.savers)
                if (saver != null &&
                    saver.SaveDataAsset(dataAssetPath) is ScriptableObject saverDataAsset &&
                    data.dataStates.Where(state => state.name == saverDataAsset.name).Count() == 0)
                    data.dataStates.Add(saverDataAsset);

            data.dataStates.RemoveAll(state => string.IsNullOrEmpty(state.name));

            if (createAsset)
            {
                if (!path.EndsWith(".asset"))
                    path += ".asset";

                AssetDatabase.CreateAsset(data, path);
            }

            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="current"></param>
        /// <param name="room"></param>
        /// <param name="i"></param>
        public override void OverwriteRoom(object current, object room, int i)
        {
            RoomStateData data = (RoomStateData)room;

            string path = AssetDatabase.GetAssetPath(data).Replace(data.name + ".asset", "");

            string newAssetPath = $"{path}/{data.name}.asset";

            CreateRoomState(data.progressionValue, newAssetPath, data);
        }

        /// <inheritdoc/>
        public override void LoadRoom(object room)
        {
            manager.UnloadAll(true);

            manager.LoadRoomFromHandlers((RoomStateData)room);
        }

        /// <inheritdoc/>
        public override void DeleteRoom(object current, object room)
        {
            RoomStateData state = (RoomStateData)room;

            RoomStateSection section = (RoomStateSection)current;

            DeleteRoomState(section, FolderUtility.GetAssetFolderPath(state), section.Rooms, section.Rooms.IndexOf(state));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalAsset"></param>
        /// <param name="path"></param>
        /// <param name="list"></param>
        /// <param name="index"></param>
        public virtual void DeleteRoomState<T>(ScriptableObject originalAsset, string path, List<T> list, int index)
        {
            AssetDatabase.DeleteAsset(path);

            CheckEmptyFolder(GetParentFolderPath(path));

            list.RemoveAt(index);

            EditorUtility.SetDirty(originalAsset);

            EditorUtility.SetDirty(builder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        protected string GetParentFolderPath(string folderPath)
            => Path.GetDirectoryName(folderPath.TrimEnd(Path.DirectorySeparatorChar));

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        protected virtual void CheckEmptyFolder(string path)
        {
            if (Directory.Exists(path) && Directory.GetFileSystemEntries(path).Length == 0)
            {
                AssetDatabase.DeleteAsset(path);

                AssetDatabase.SaveAssets();
            }
        }


        /// <inheritdoc/>
        public override void DisplayRoomPieces(object RoomStateData)
        {
            RoomStateData room = (RoomStateData)RoomStateData;

            room.SetPersistent(HandyFields.BoolField(room.isPersistent, "Is Persistent: "));

            foreach (IRoomStateLoader loader in manager.loaders)
                if (GUILayout.Button("Load " + loader.GetTypeName(), GUILayout.Width(150)))
                    loader.LoadData(room.dataStates);
        }
    }
}
#endif