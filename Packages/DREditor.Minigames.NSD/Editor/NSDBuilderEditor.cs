//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor;
using DREditor.Editor.Dialogues;
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Editor;
using DREditor.Minigames.NSD.Managers;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.NSD.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(NSDBuilder))]
    public class NSDBuilderEditor : DebateTypeEditor<Panel>
    {
        /// <summary>
        ///
        /// </summary>
        protected NSDBuilder builder;

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
        protected readonly PanelLineEditor<Panel> lineEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly TrialDialogueDebugToolEditor<Panel> debugTool = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly int guiSpacing = 10;


        protected override LineEditorBase LineEditor { get => lineEditor; }

        protected override DialogueDebugToolEditorBase DebugTool { get => debugTool; }


        protected virtual void OnEnable()
        {
            bulletDifficultyEditor = new();

            timerDifficultyEditor = new();

            SetDialogue(target as NSDBuilder);

            lineEditor.Initialize(dia, serializedObject, this);
        }

        /// <inheritdoc/>
        protected override void SetDialogue(DialogueBase<Panel> dia)
        {
            base.SetDialogue(dia);

            builder = dia as NSDBuilder;
        }

        /// <inheritdoc/>
        protected override bool DatabasesAreValid()
        {
            if (!base.DatabasesAreValid())
                return false;

            if (EditPanelMode)
                lineEditor.EditPanelModeWindow(dia.Lines[editPanelNum]);
            
            return !EditPanelMode;
        }

        /// <inheritdoc/>
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
            if (!EditPanelMode)
            {
                for (int i = 0; i < dia.Lines.Count; i++)
                {
                    if (dia.Lines[i].Edit)
                    {
                        EditPanelMode = true;

                        EditPanelNum = i;
                    }
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

            DisplayEndDialogue();

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
        protected virtual void DisplayEndDialogue()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Ending Panel Group Dialogue :", GUILayout.Width(180));

                builder.EndDialogue = HandyFields.UnityField(builder.EndDialogue, 170, 20);
            }
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