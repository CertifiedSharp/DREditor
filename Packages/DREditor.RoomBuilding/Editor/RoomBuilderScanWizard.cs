//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.Progressing;
using UnityEditor;
using UnityEngine;

namespace DREditor.RoomBuilding.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public class RoomBuilderScanWizard : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        protected ProgressionDatabase progressionDatabase;

        /// <summary>
        ///
        /// </summary>
        protected int chapterIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected Chapter chapter;

        /// <summary>
        ///
        /// </summary>
        protected int objectiveIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected Objective objective;



        /// <inheritdoc/>
        protected override bool DrawWizardGUI()
        {
            InitializeProgression();

            DisplayProgressionGUI();

            return base.DrawWizardGUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeProgression()
        {
            if (progressionDatabase == null)
                progressionDatabase = DatabaseUtility.LoadDatabase<ProgressionDatabase>();

            if (progressionDatabase == null)
            {
                UnityEngine.Debug.LogError("The Progression Database could not be found");

                Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayProgressionGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                chapterIndex = HandyFields.IntField("Chapter:", chapterIndex, 50, 8, false);

                chapter = progressionDatabase.Chapters[chapterIndex];

                objectiveIndex = EditorGUILayout.Popup(objectiveIndex, chapter.GetObjectives(), GUILayout.Width(200));

                objective = chapter.Objectives[objectiveIndex];

                GUILayout.FlexibleSpace();
            }
        }
    }
}
#endif