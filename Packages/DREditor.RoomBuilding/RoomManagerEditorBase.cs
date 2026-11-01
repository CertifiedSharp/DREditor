//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Progressing;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RoomManagerEditorBase : RoomManagerEditorCoreBase
    {
        /// <inheritdoc/>
        protected override void CreateForm()
        {
            DisplayEvents();

            if (DisplayBuilderField())
            {
                DisplayChapter();

                DisplayBuilderData();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract bool DisplayBuilderField();

        /// <summary>
        ///
        /// </summary>
        protected abstract bool IsManagerBuilderValid();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBuilderData()
        {
            if (IsManagerBuilderValid())
            {
                CreateRoomSave();

                ClearDropDown();

                DisplayRoomStates();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateRoomSave()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (managerBase.Chapter == null)
                    managerBase.SetChapter(pDB.Chapters[managerBase.intChapter]);

                intObjective = EditorGUILayout.Popup(intObjective, managerBase.Chapter.GetObjectives());

                if (GUILayout.Button("Create Room Save"))
                    SaveRoom(pDB.Chapters[managerBase.intChapter].Objectives[intObjective]);
            }

            DisplayShellData();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayShellData();

        /// <summary>
        ///
        /// </summary>
        protected abstract void SaveRoom(Objective pValue);

        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayRoomStates();
    }
}
#endif