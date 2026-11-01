//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(FTEDatabase))]
    public class FTEDatabaseEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected FTEDatabase database;

        /// <summary>
        ///
        /// </summary>
        protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty propName;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            database = target as FTEDatabase;

            propName = serializedObject.FindProperty("freeTimeCharacterDialogues");
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CreateForm();

            EditorUtility.SetDirty(database);

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            if (!DatabaseUtility
                .InitializeDatabase(characterDatabase, out characterDatabase))
                return;

            if (GUILayout.Button("Generate Base Structure", GUILayout.Width(200)))
                if (EditorUtility.DisplayDialog("Generate",
                    "Are you sure you want to Generate the data? This will delete the current data.",
                                "Yes", "No"))
                    GenerateData();

            GUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope("Box"))
                HandyFields.HardPropertyField("List: ", propName, 500);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void GenerateData()
        {
            database.freeTimeCharacterDialogues.Clear();

            List<Character> characters = characterDatabase.GetFTECharacterList();

            foreach (Character character in characters)
            {
                FTECharacterDialogues fte = CustomEditorUtility
                    .CreateAssetWithFolders<FTECharacterDialogues>(character.FirstName + "FTEDialogues");

                fte.SetCharacter(character);

                fte.SetDialogues(new List<Dialogues.Dialogue>(new Dialogues.Dialogue[]
                    {null, null, null, null, null}));

                AssetDatabase.SaveAssets();

                database.freeTimeCharacterDialogues.Add(fte);
            }
        }
    }
}
#endif