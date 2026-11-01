//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Minigames.Debates.Managers;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PanelLineEditor<T> : TrialLineEditor<T> where T : Panel, new()
    {
        /// <summary>
        ///
        /// </summary>
        protected IDebateEditor debateEditor;

        /// <summary>
        ///
        /// </summary>
        protected IDebateLineManager lineManager;

        /// <summary>
        ///
        /// </summary>
        protected readonly PhrasePresetEditor presetEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly PanelPhrasesEditor<T> phrasesEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected T panel;


        public override void Initialize(DialogueCore dia, SerializedObject serializedObject, UnityEditor.Editor parentEditor,
            SerializedProperty linesProperty = null, List<T> lines = null)
        {
            base.Initialize(dia, serializedObject, parentEditor, linesProperty, lines);

            debateEditor = parentEditor as IDebateEditor;

            if (Application.isPlaying)
                lineManager ??= UnityEngine.Object.FindObjectsByType<MonoBehaviour>()
                    .OfType<IDebateLineManager>()
                    .FirstOrDefault();
        }

        protected override void OnTestButtonPressed()
        {
            base.OnTestButtonPressed();

            TestPanel(Lines[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        protected virtual void TestPanel(T panel)
        {
            if (InPlayMode())
                lineManager?.PlayTestPanel(panel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InPlayMode()
        {
            if (!Application.isPlaying)
                EditorUtility.DisplayDialog("ALERT: Must be in Play Mode",
                    "PROPOSAL: Enter play mode with a DRTrialCamera to test lines.", "Ok");

            return Application.isPlaying;
        }


        protected override void TestButton()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                    base.TestButton();

                DisplayEditPanelButton();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayEditPanelButton()
        {
            if (debateEditor != null && HandyFields.Button("Edit Panel"))
            {
                debateEditor.EditPanelNum = i;

                debateEditor.EditPanelMode = true;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        public virtual void EditPanelModeWindow(T panel)
        {
            if (debateEditor == null)
            {
                EditorGUILayout.HelpBox("Debate editor not available", MessageType.Error);

                return;
            }

            this.panel = panel;

            presetEditor.Initialize();

            phrasesEditor.Initialize(panel, debateEditor);

            DisplayExitPanelWindowButton();

            GUILayout.Space(PanelEditorConstants.EditorSpacing);

            DisplayPanelData();

            GUILayout.Space(PanelEditorConstants.EditorSpacing);

            DisplayTestPanelButton();

            GUILayout.Space(PanelEditorConstants.EditorSpacing);

            DisplayFocusPhraseButtons();

            DisplayPhrases();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayExitPanelWindowButton()
        {
            if (HandyFields.Button("Exit Panel Editor"))
            {
                debateEditor.EditPanelMode = false;

                panel.Edit = debateEditor.EditPanelMode;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPanelData()
        {
            if (debateEditor.IsFocusingPhrase())
                return;

            using (new EditorGUILayout.HorizontalScope())
            {
                panel.SetText(HandyFields.StringArea("Panel Text:", panel.Text));

                panel.WaitTime =
                    HandyFields.FloatField("Wait Time after animations finish: ", panel.WaitTime, (int)PanelEditorConstants.EditorSpacing);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTestPanelButton()
        {
            if (HandyFields.Button("Test Panel"))
            {
                if (lineManager != null)
                    TestPanel(panel);
                else
                    UnityEngine.Debug.LogWarning("ALERT: There is no line manager in the scene inheriting type: "
                    + nameof(IDebateLineManager));
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void DisplayFocusPhraseButtons()
        {
            if (debateEditor.IsFocusingPhrase())
            {
                GUILayout.Space(PanelEditorConstants.EditorSpacing / PanelEditorConstants.DivideSpacing);

                DisplayFocusMoveButtons();

                GUILayout.Space(PanelEditorConstants.EditorSpacing);
            }

            if (debateEditor.IsFocusingPhrase())
                phrasesEditor.DisplayPhrase(debateEditor.FocusPhraseIndex);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFocusMoveButtons()
        {
            using (new EditorGUILayout.HorizontalScope("Box"))
            {
                EditorGUILayout.BeginHorizontal();

                DisplayDirectionButton("Previous Phrase", debateEditor.FocusPhraseIndex > 0, -1);

                GUILayout.FlexibleSpace();

                DisplayDirectionButton("Next Phrase", debateEditor.FocusPhraseIndex < panel.Phrases.Count - 1, 1);

                GUILayout.FlexibleSpace();

                DisplayExitFocusButton();

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="enableCondition"></param>
        /// <param name="moveDirectionAmount"></param>
        protected virtual void DisplayDirectionButton(string buttonText, bool enableCondition, int moveDirectionAmount)
        {
            using (new EditorGUI.DisabledScope(!enableCondition))
                if (HandyFields.Button(buttonText))
                    debateEditor.SetFocusPhrase(debateEditor.FocusPhraseIndex + moveDirectionAmount);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayExitFocusButton()
        {
            if (debateEditor.IsFocusingPhrase() && HandyFields.Button("Exit Focused Phrase"))
                debateEditor.SetFocusPhrase(-1);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhrases()
        {
            phrasesEditor.DisplayPhrases();

            GUILayout.Space(PanelEditorConstants.EditorSpacing);

            phrasesEditor.DisplayCreatePhraseToolbar();
        }
    }
}