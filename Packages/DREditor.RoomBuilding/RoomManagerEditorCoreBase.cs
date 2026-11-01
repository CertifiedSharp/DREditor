//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Progressing;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(RoomManagerBase))]
    [CanEditMultipleObjects]
    public abstract class RoomManagerEditorCoreBase : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected ProgressionDatabase pDB;

        /// <summary>
        ///
        /// </summary>
        protected RoomManagerBase managerBase;

        /// <summary>
        ///
        /// </summary>
        protected bool clearFold = false;

        /// <summary>
        ///
        /// </summary>
        protected int intObjective = 0;

        /// <summary>
        ///
        /// </summary>
        protected bool roomFold = true;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            managerBase = (RoomManagerBase)target;

            try
            {
                pDB = (ProgressionDatabase)Resources.Load("Progression/ProgressionDatabase");
            }
            catch
            {
                UnityEngine.Debug.LogError(
                    "Progression asset is missing or incorrectly placed. " +
                    "Ensure it's located in: Resources/Progression and named ProgressionDatabase."
                );
            }
        }


        /// <summary>
        ///
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CreateForm();

            SetBuilderDirty();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void CreateForm();

        /// <summary>
        ///
        /// </summary>
        protected abstract void SetBuilderDirty();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayChapter()
        {
            managerBase.SetIntChapter(HandyFields.IntField("Chapter: ", managerBase.intChapter, 30));

            if (!IsChapterValid())
                return;

            managerBase.SetChapter(pDB.Chapters[managerBase.intChapter]);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual bool IsChapterValid()
        {
            if (managerBase.intChapter < 0 || managerBase.intChapter >= pDB.Chapters.Count)
            {
                GUILayout.Label("Please enter a valid chapter number.");

                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayEvents()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("OnRoomInstanceLoaded: ", GUILayout.MaxWidth(160));

                managerBase.SetOnRoomInstanceLoaded(HandyFields.UnityField(managerBase.onRoomInstanceLoaded, 170, 30));
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearDropDown()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                clearFold = EditorGUILayout.Foldout(clearFold, "Clear Objects");

                if (GUILayout.Button("Clear All", GUILayout.Width(150)) &&
                    EditorUtility.DisplayDialog("Clear All",
                        "Are you sure you want to clear all saveable objects?",
                        "Yes", "No"))
                    ClearAll();
            }

            if (clearFold)
                OnClearFoldOut();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnClearFoldOut();

        /// <summary>
        ///
        /// </summary>
        protected abstract void ClearAll();
    }
}
#endif