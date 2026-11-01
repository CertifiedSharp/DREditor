// Original Author: KHeartz
// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    [CustomEditor(typeof(PresentDatabase))]
    public class PresentDatabaseEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected PresentDatabase database;


        protected virtual void OnEnable() => database = target as PresentDatabase;

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            CreateForm();

            EditorUtility.SetDirty(database);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            DisplayHeader();

            RestorePresentIDsFromImageButton();

            if (ImportPresents())
                return;

            ClearPresentsButton();

            if (AddNewPresent())
                return;

            DisplayPresentPreviews();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayHeader()
        {
            HandyFields.Label("Present Database");

            if (database != null && database.presents != null)
                HandyFields.Label("Present Count: " + database.presents.Count);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ImportPresents()
        {
            bool result = HandyFields.Button("Import Presents", "Adds all currently selected Present assets in the project to the database.");

            if (result)
                foreach (var present in Selection.objects)
                    database.presents.Add((Present)present);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearPresentsButton()
        {
            if (HandyFields.Button("Clear Presents") &&
                EditorUtility.DisplayDialog("Clear Presents", "Are you sure you want to clear presents?", "Yes", "No"))
                database.presents.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AddNewPresent()
        {
            if (HandyFields.Button("Add New Present"))
                database.presents.Add(CreateInstance<Present>());

            return database.presents == null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPresentPreviews()
        {
            if (database != null && database.presents != null)
                for (int i = 0; i < database.presents.Count; i++)
                    using (new GUILayout.HorizontalScope("Box"))
                        DisplayPresentPreview(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayPresentPreview(int i)
        {
            ShowPresentImage(database.presents[i]);

            SelectPresent(database.presents[i], i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="present"></param>
        protected virtual void ShowPresentImage(Present present)
        {
            using (new EditorGUILayout.VerticalScope())
                GUILayout.Label(AssetPreview.GetAssetPreview(present.Image));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="present"></param>
        /// <param name="index"></param>
        protected virtual void SelectPresent(Present present, int index)
        {
            GUILayout.FlexibleSpace();

            database.presents[index] = HandyFields.UnityField(present, 200, 40);

            RemovePresentButton(index);

            GUILayout.FlexibleSpace();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        protected virtual void RemovePresentButton(int index)
        {
            if (HandyFields.Button("X"))
                database.presents.RemoveAt(index);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RestorePresentIDsFromImageButton()
        {
            if (HandyFields.Button("Restore Present IDs from their image"))
                RestorePresentIDsFromImage();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RestorePresentIDsFromImage()
        {
            foreach (Present present in database.presents)
            {
                if (present.Image != null)
                {
                    present.ID = Int32.Parse(Regex.Match(present.Image.name, @"\d+").Value);

                    EditorUtility.SetDirty(present);
                }
            }
        }
    }
}

#endif