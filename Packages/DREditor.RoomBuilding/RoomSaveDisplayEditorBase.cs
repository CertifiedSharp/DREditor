//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RoomSaveDisplayEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        public List<bool> saveFolds = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentSection"></param>
        /// <param name="i"></param>
        /// <param name="room"></param>
        /// <param name="foldoutTitle"></param>
        public virtual void DisplayRoomSave(object currentSection, int i, object room, string foldoutTitle)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                saveFolds[i] = EditorGUILayout.Foldout(saveFolds[i], foldoutTitle);

                if (LoadRoomButtonPressed())
                    LoadRoom(room);

                if (OverwriteRoomButtonPressed())
                    OverwriteRoom(currentSection, room, i);

                if (DeleteRoomButtonPressed())
                    DeleteRoom(currentSection, room);

                DisplayPingButton(room);
            }

            if (saveFolds[i])
                DisplayRoomPieces(room);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool LoadRoomButtonPressed() => GUILayout.Button("Load Room");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        public abstract void LoadRoom(object room);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool OverwriteRoomButtonPressed() =>
            GUILayout.Button("Overwrite Room") &&
            EditorUtility.DisplayDialog("Overwrite Save", "Are you sure you want to overwrite the room data?", "Yes", "No");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="room"></param>
        /// <param name="i"></param>
        public abstract void OverwriteRoom(object current, object room, int i); // Sweden Note: After RoomManager removal remove i parameter


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool DeleteRoomButtonPressed() =>
            GUILayout.Button("Delete Room") &&
            EditorUtility.DisplayDialog("Delete Save", "Are you sure you want to delete the room data?", "Yes", "No");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="room"></param>
        public abstract void DeleteRoom(object current, object room);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        protected virtual void DisplayPingButton(object room)
        {
            if (room is UnityEngine.Object pingableRoom && GUILayout.Button("☼"))
                EditorGUIUtility.PingObject(pingableRoom);
        }

        /// <summary>
        ///
        /// </summary>
        public abstract void DisplayRoomPieces(object roomData);
    }
}
#endif