//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Progressing;
using DREditor.RoomStateBuilding;
using DREditor.Editor.Utility;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(RoomStateManager))]
    [CanEditMultipleObjects]
    public class RoomStateManagerEditor : RoomManagerEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected RoomStateManager manager;

        /// <summary>
        ///
        /// </summary>
        protected RoomStateBuilder builder;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty monoBehavioursProp;

        /// <summary>
        ///
        /// </summary>
        protected RoomStateShellDisplayEditor shellEditor;

        /// <summary>
        ///
        /// </summary>
        protected RoomStateSaveDisplayEditor roomStateEditor;


        #region Handler Initialization

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            manager = (RoomStateManager)target;

            manager.InitializeHandlers();

            monoBehavioursProp = serializedObject.FindProperty("loadHandlers");

            base.OnEnable();

            UpdateRoomBuilderField(manager);

            roomStateEditor = new(manager, manager.Builder);

            shellEditor = new(manager, manager.Builder, roomStateEditor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="manager"></param>
        static void UpdateRoomBuilderField(RoomStateManager manager)
        {
            if (!(!Application.isPlaying && EditorUtility.IsPersistent(manager)) && manager.Builder == null)
            {
                UnityEngine.Debug.Log("Checking if RoomStateBuilder Already exists");

                var found = AssetDatabase.FindAssets("t:RoomStateBuilder");

                if (found.Length != 0)
                {
                    foreach (var asset in found)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(asset);

                        var roomStateBuilder = AssetDatabase.LoadAssetAtPath<RoomStateBuilder>(path);

                        if (roomStateBuilder != null && roomStateBuilder.name == EditorSceneManager.GetActiveScene().name)
                            manager.SetBuilder(roomStateBuilder);
                    }

                    if (manager.Builder == null)
                        UnityEngine.Debug.Log("Loaded but couldn't find.");

                    EditorUtility.SetDirty(manager);
                }
            }
        }

        #endregion

        #region Builder Initialization

        /// <inheritdoc/>
        protected override bool DisplayBuilderField()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                manager.SetBuilder(HandyFields.UnityField(manager.Builder, 200, 35));

                if (IsManagerBuilderValid() && builder == null)
                    builder = manager.Builder;

                if (builder == null)
                    if (GUILayout.Button("Initialize Builder"))
                        InitializeBuilder();
                    else
                        return false;

                if (builder != null && (builder.Sections.Count == 0 || AllSectionsAreNull()))
                    if (GUILayout.Button("Initialize Sections"))
                        InitializeSections();
                    else
                        return false;

                if (GUILayout.Button("Apply Changes"))
                    EditorUtility.SetDirty(manager);

                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AllSectionsAreNull()
        {
            foreach (var section in builder.Sections)
                if (section != null)
                    return false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeBuilder()
        {
            var builderAsset = ScriptableObject.CreateInstance<RoomStateBuilder>();

            string buildFolderPath = "Assets/RoomBuilding";

            if (!FolderUtility.ProjectFolderExists(buildFolderPath))
                AssetDatabase.CreateFolder("Assets", "RoomBuilding");

            string buildFolder = AssetDatabase.CreateFolder(buildFolderPath, SceneManager.GetActiveScene().name);

            if (buildFolder == "")
            {
                UnityEngine.Debug.LogWarning("Could not create folder. Canceling Initialization.");

                UnityEngine.Debug.Log("Builder Path: " + buildFolderPath);

                return;
            }

            string newFolderPath = AssetDatabase.GUIDToAssetPath(buildFolder);

            AssetDatabase.CreateAsset(builderAsset, $"{newFolderPath}/{SceneManager.GetActiveScene().name}.asset");

            AssetDatabase.SaveAssets();

            builder = builderAsset;

            manager.SetBuilder(builder);

            SetBuilderDirty();

            EditorUtility.SetDirty(manager);

            InitializeSections();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSections()
        {
            foreach (Chapter chapter in pDB.Chapters)
            {
                var sectionAsset = ScriptableObject.CreateInstance<RoomStateSection>();

                sectionAsset.SetChapter(chapter);

                string builderPath = AssetDatabase.GetAssetPath(builder);

                string builderPrefix = builder.name.Replace(".asset", "");

                string sectionPath = builderPath.Replace("/" + builder.name + ".asset", "");

                string sectionFolderName = builderPrefix + $"_{pDB.GetChapterIndex(chapter)}";

                string sectionFolder = AssetDatabase.CreateFolder(sectionPath, sectionFolderName);

                if (sectionFolder == "")
                {
                    UnityEngine.Debug.LogWarning("Could not create folder. Canceling Initialization.");

                    UnityEngine.Debug.Log("Builder Path: " + sectionPath);

                    return;
                }

                string newFolderPath = AssetDatabase.GUIDToAssetPath(sectionFolder);

                string newAssetPath = $"{newFolderPath}/{builderPrefix}_{pDB.GetChapterIndex(chapter)}.asset";

                AssetDatabase.CreateAsset(sectionAsset, newAssetPath);

                AssetDatabase.SaveAssets();

                EditorUtility.SetDirty(sectionAsset);

                builder.Sections.Add(sectionAsset);

                SetBuilderDirty();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayHandlers()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(monoBehavioursProp, true);

            manager.InitializeHandlers();

            serializedObject.ApplyModifiedProperties();
        }

        /// <inheritdoc/>
        protected override bool IsManagerBuilderValid() => manager.Builder != null;

        /// <inheritdoc/>
        protected override void DisplayChapter()
        {
            DisplayHandlers();

            base.DisplayChapter();
        }

        /// <inheritdoc/>
        protected override void SetBuilderDirty()
        {
            if (builder != null)
                EditorUtility.SetDirty(builder);
        }

        #endregion

        #region Create Room Save

        /// <inheritdoc/>
        protected override void SaveRoom(Objective pValue)
        {
            var look = builder.Sections.Where(n => n.Chapter == managerBase.Chapter).ToList();

            int index = manager.intChapter;

            if (look.Count > 0)
                index = builder.Sections.IndexOf(look[0]);

            string sectionPath = FolderUtility.GetAssetFolderPath(builder.Sections[index]);

            string sectionPrefix = builder.Sections[index].name.Replace(".asset", "");

            string roomStateName = sectionPrefix + "_" + pValue.Description;

            if (!FolderUtility.TryCreateFolder(ref sectionPath, roomStateName))
                return;

            string newAssetPath = $"{sectionPath}/{roomStateName}.asset";

            RoomStateData room = roomStateEditor.CreateRoomState(pValue, newAssetPath);

            UnityEngine.Debug.Log(JsonUtility.ToJson(room));

            builder.Sections[index].Rooms.Add(room);

            EditorUtility.SetDirty(builder.Sections[index]);
        }

        #endregion

        #region Clear Drop Down

        /// <inheritdoc/>
        protected override void OnClearFoldOut()
        {
            foreach (IRoomStateLoader loader in manager.loaders)
                if (GUILayout.Button("Unload " + loader.GetTypeName(), GUILayout.Width(150)))
                    if (EditorUtility.DisplayDialog($"Unload {loader.GetTypeName()}",
                        $"Are you sure you want to destroy/unload all {loader.GetTypeName()}?",
                        "Yes", "No"))
                    {
                        loader.Unload(true);

                        EditorUtility.SetDirty(manager);
                    }
        }

        /// <inheritdoc/>
        protected override void ClearAll()
        {
            manager.UnloadAll(true);

            EditorUtility.SetDirty(manager);
        }

        #endregion

        #region Display Room States

        /// <inheritdoc/>
        protected override void DisplayShellData() => shellEditor.DisplayShellData();

        /// <inheritdoc/>
        protected override void DisplayRoomStates()
        {
            RoomStateSection current = null;

            for (int i = 0; i < builder.Sections.Count; i++)
            {
                RoomStateSection roomSection = builder.Sections[i];

                if (roomSection == null)
                {
                    builder.Sections.Remove(roomSection);

                    EditorUtility.SetDirty(builder);

                    AssetDatabase.SaveAssetIfDirty(builder);

                    continue;
                }

                if (roomSection.Chapter == pDB.Chapters[pDB.Chapters.IndexOf(managerBase.Chapter)])
                {
                    current = roomSection;

                    break;
                }
            }

            if (current == null)
                GUILayout.Label("Couldn't find a Room Builder Section with the related chapter. " +
                    "Please do not create a room save while this is showing.");
            else
                DisplayRoomSaves(current);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="current"></param>
        protected virtual void DisplayRoomSaves(RoomStateSection current)
        {
            if (current.Rooms.Count > 0)
            {
                if (roomStateEditor.saveFolds.Count != current.Rooms.Count)
                    for (int i = 0; i < current.Rooms.Count; i++)
                        roomStateEditor.saveFolds.Add(false);

                roomFold = EditorGUILayout.Foldout(roomFold, "Saved Rooms");

                if (roomFold)
                {
                    GUILayout.Label("On Load will remove REFERENCED Actors");

                    for (int i = 0; i < current.Rooms.Count; i++)
                        roomStateEditor.DisplayRoomSave(current, i, current.Rooms[i], current.Rooms[i].progressionValue.Description);
                }
            }
        }

        #endregion
    }
}
#endif