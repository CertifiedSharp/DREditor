//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Progressing;
using DREditor.RoomBuilding.Implementation;
using DREditor.Editor.Utility;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(RoomManager))]
    [CanEditMultipleObjects]
    public class RoomManagerEditor : RoomManagerEditorBase
    {
        RoomManager manager;

        RoomBuilder builder;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            manager = (RoomManager)target;

            managerBase = manager;

            editor = new(manager);
            

            base.OnEnable();

            shellEditor = new(manager, manager.Builder);
        }

        #region Builder Initialization

        /// <inheritdoc/>
        protected override void SetBuilderDirty()
        {
            if (manager.Builder != null)
                EditorUtility.SetDirty(builder);
        }

        /// <inheritdoc/>
        protected override bool DisplayBuilderField()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                manager.SetBuilder(HandyFields.UnityField(manager.Builder, 200, 35));

                if (manager.Builder != null && builder == null)
                    builder = manager.Builder;

                if (GUILayout.Button("Apply Changes"))
                    EditorUtility.SetDirty(manager);

                return true;
            }
        }

        /// <inheritdoc/>
        protected override bool IsManagerBuilderValid() => manager.Builder != null;

        #endregion

        #region Create Room Save

        /// <inheritdoc/>
        protected override void DisplayShellData()
        {
            shellEditor.DisplayShellData();
        }

        /// <inheritdoc/>
        protected override void SaveRoom(Objective pValue)
        {
            RoomData room = manager.NewSaveRoom(pValue);

            UnityEngine.Debug.Log(JsonUtility.ToJson(room));

            var look = builder.Sections.Where(n => n.Chapter == managerBase.Chapter).ToList();

            int index = managerBase.intChapter;

            if (look.Count > 0)
                index = builder.Sections.IndexOf(look[0]);

            builder.Sections[index].Rooms.Add(room);
        }

        #endregion

        #region Clear Drop Down

        /// <inheritdoc/>
        protected override void OnClearFoldOut()
        {
            if (GUILayout.Button("Unload Spawnables", GUILayout.Width(150)))
                if (EditorUtility.DisplayDialog("Unload Spawnables", "Are you sure you want to destroy all Spawnables?",
                    "Yes", "No"))
                {
                    SpawnableLoader.UnloadSpawnables(true);
                    EditorUtility.SetDirty(manager);
                }

            if (GUILayout.Button("Unload Actors", GUILayout.Width(150)))
                if (EditorUtility.DisplayDialog("Unload Actors",
                    "Warning! All REFERENCED objects with the Actor component will be destroyed. Are you sure?",
                    "Yes", "No"))
                {
                    ActorLoader.UnloadActors(true);
                    EditorUtility.SetDirty(manager);
                }

            if (GUILayout.Button("Unload Actuators", GUILayout.Width(150)))
                if (EditorUtility.DisplayDialog("Clear Actuators", "Are you sure you want to clear all Actuators?",
                    "Yes", "No"))
                {
                    TriggerLoader.UnloadEvents();
                    EditorUtility.SetDirty(manager);
                }

            if (GUILayout.Button("Clear Door Locks", GUILayout.Width(150)))
                if (EditorUtility.DisplayDialog("Clear Door Locks", "Are you sure you want to clear Door Locks?",
                    "Yes", "No"))
                {
                    DoorLoader.UnloadDoors();
                    EditorUtility.SetDirty(manager);
                }

            if (GUILayout.Button("Clear Items", GUILayout.Width(150)))
                if (EditorUtility.DisplayDialog("Clear Items", "Are you sure you want to clear all Item Data?",
                    "Yes", "No"))
                {
                    ItemActorLoader.UnloadItems();
                    EditorUtility.SetDirty(manager);
                }
        }

        /// <inheritdoc/>
        protected override void ClearAll()
        {
            SpawnableLoader.UnloadSpawnables(true);

            ActorLoader.UnloadActors(true);

            ItemActorLoader.UnloadItems();

            TriggerLoader.UnloadEvents();

            DoorLoader.UnloadDoors();

            EditorUtility.SetDirty(manager);
        }

        #endregion

        #region Load Room Save

        /// <inheritdoc/>
        protected override void DisplayRoomStates()
        {
            RoomSection current = null;

            foreach (RoomSection r in builder.Sections)
            {
                if (r.Chapter == pDB.Chapters[pDB.Chapters.IndexOf(managerBase.Chapter)])
                {
                    current = r;

                    break;
                }
            }

            if (current == null)
            {
                GUILayout.Label("Couldn't find a Room Builder Section with the related chapter. " +
                    "Please do not create a room save while this is showing.");

                return;
            }

            if (current.Rooms.Count > 0)
            {
                if (editor.saveFolds.Count != current.Rooms.Count)
                    for (int i = 0; i < current.Rooms.Count; i++)
                        editor.saveFolds.Add(false);

                string foldTitle = "Saved Rooms";

                roomFold = EditorGUILayout.Foldout(roomFold, foldTitle);

                if (roomFold)
                {
                    GUILayout.Label("On Load will remove REFERENCED Actors");

                    for (int i = 0; i < current.Rooms.Count; i++)
                        editor.DisplayRoomSave(current, i, current.Rooms[i], current.Rooms[i].progressionValue.Description);
                }
            }
        }

        RoomManagerShellDisplayEditor shellEditor;
        RoomManagerDisplayEditor editor;
        #endregion
    }
}
#endif