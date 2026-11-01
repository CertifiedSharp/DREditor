//Author: Benjamin "Sweden" Jillson : sweden_gaming For Project Eden's Garden
using DREditor.Editor.Dialogues;
using DREditor.Minigames.Debates.Editor;
using DREditor.Minigames.RS.Managers;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.RS.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RSEditorBase<T> : DebateTypeEditor<T> where T : RSGroup, new()
    {
        /// <summary>
        ///
        /// </summary>
        protected RSBuilder builder;

        /// <summary>
        ///
        /// </summary>
        protected BulletDifficultyEditor bulletDifficultyEditor;

        /// <summary>
        ///
        /// </summary>
        protected TimedDifficultyEditor timerDifficultyEditor;

        /// <summary>
        ///
        /// </summary>
        protected int currentSection;

        /// <summary>
        ///
        /// </summary>
        protected bool editResponsePanel = false;


        /// <summary>
        ///
        /// </summary>
        protected readonly MultiPanelLineEditor<RSPanel> lineEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly RSDebugToolEditor debugTool = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly int guiSpacing = 10;


        /// <summary>
        ///
        /// </summary>
        public int CurrentSection { get => currentSection; set => currentSection = value; }

        /// <summary>
        ///
        /// </summary>
        public bool EditResponsePanel { get => editResponsePanel; set => editResponsePanel = value; }


        protected override LineEditorBase LineEditor { get => lineEditor; }

        protected override DialogueDebugToolEditorBase DebugTool { get => debugTool; }


        protected virtual void OnEnable()
        {
            builder = target as RSBuilder;

            bulletDifficultyEditor = new();

            timerDifficultyEditor = new();
        }

        protected override bool DatabasesAreValid()
        {
            if (!base.DatabasesAreValid())
                return false;

            if (EditPanelMode)
            {
                var section = dia.Lines[CurrentSection];

                var panel = !EditResponsePanel ? section.Panels[editPanelNum] : section.ResponsePanels[editPanelNum];

                lineEditor.EditPanelModeWindow(panel);
            }
            else
                EditResponsePanel = false;

            return !EditPanelMode;
        }

        protected override void SetWordWrappingColor()
        {
            base.SetWordWrappingColor();

            CheckPanelMode();

            DisplayBuilder();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckPanelMode()
        {
            if (EditPanelMode || dia == null || dia.Lines == null || CurrentSection < 0 || CurrentSection >= dia.Lines.Count)
                return;

            var section = dia.Lines[CurrentSection];

            if (section?.Panels == null)
                return;

            for (int i = 0; i < section.Panels.Count; i++)
            {
                if (section.Panels[i]?.Edit == true)
                {
                    EditPanelMode = true;

                    EditPanelNum = i;

                    break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBuilder()
        {
            if (lineEditor.TestLine == -1)
                DisplayDebateSpecifics();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDebateSpecifics()
        {
            GUILayout.Space(guiSpacing);

            DisplayStartingMusic();

            GUILayout.Space(guiSpacing);

            DisplayDifficultyEditors();

            GUILayout.Space(guiSpacing);

            DisplaySkipField();

            GUILayout.Space(guiSpacing);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDifficultyEditors()
        {
            using (new EditorGUILayout.VerticalScope("Box"))
                timerDifficultyEditor.DisplaySettings(builder.times);

            using (new EditorGUILayout.VerticalScope("Box"))
                if (evidenceDatabase.Evidences[builder.trialNumber] != null)
                    bulletDifficultyEditor?.DisplaySettings(builder.TruthBullets, ref builder.trialNumber);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySkipField()
        {
            if (lineEditor.TestLine == -1)
                builder.Skip = HandyFields.IntField(new GUIContent("Skip:",
                    "Amount of trial sequences to be skipped when this minigame completes."), builder.Skip);
        }
    }
}