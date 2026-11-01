//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RoomShellDisplayEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        public virtual void DisplayShellData()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Shell", GUILayout.Width(100));

                if (HasShellData())
                {
                    if (GUILayout.Button("Load Shell"))
                        LoadShell();

                    if (GUILayout.Button("Overwrite Shell") &&
                        EditorUtility.DisplayDialog("Overwrite Save",
                            "Are you sure you want to overwrite the shell room data?",
                            "Yes", "No"))
                        OverwriteShell();

                    if (GUILayout.Button("Delete Shell") &&
                        EditorUtility.DisplayDialog("Clear Shell",
                            "Are you sure you want to clear this room's shell?", "Yes", "No"))
                        DeleteShell();
                }
                else if (GUILayout.Button("Create Shell Save", GUILayout.Width(175)))
                    CreateShellSave();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract bool HasShellData();

        /// <summary>
        ///
        /// </summary>
        protected abstract void CreateShellSave();

        /// <summary>
        ///
        /// </summary>
        protected abstract void LoadShell();

        /// <summary>
        ///
        /// </summary>
        protected abstract void OverwriteShell();

        /// <summary>
        ///
        /// </summary>
        protected abstract void DeleteShell();
    }
}
#endif