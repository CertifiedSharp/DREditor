//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Investigations;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DebateTypeEditor<T> : DialogueEditorBase<T>, IDebateEditor where T : new()
    {
        /// <summary>
        ///
        /// </summary>
        protected TrialCameraAnimDatabase cameraAnimDatabase;

        /// <summary>
        ///
        /// </summary>
        protected EvidenceDatabase evidenceDatabase;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty music;

        /// <summary>
        ///
        /// </summary>
        protected DialogueCore conversionDialogue;

        /// <summary>
        ///
        /// </summary>
        protected bool editPanelMode = false;

        /// <summary>
        ///
        /// </summary>
        protected int editPanelNum;

        /// <summary>
        ///
        /// </summary>
        protected int focusPhraseIndex = -1;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty iterator;


        /// <summary>
        ///
        /// </summary>
        private const string MusicFieldName = "StartingMusic";


        public bool DebugMode { get => debugMode; set => debugMode = value; }

        public bool EditPanelMode { get => editPanelMode; set => editPanelMode = value; }

        public int EditPanelNum { get => editPanelNum; set => editPanelNum = value; }

        public int FocusPhraseIndex { get => focusPhraseIndex; }

        public virtual void SetFocusPhrase(int focusPhraseIndex) => this.focusPhraseIndex = focusPhraseIndex;

        public virtual bool IsFocusingPhrase() => focusPhraseIndex != -1;


        protected override bool DatabasesAreValid()
        {
            if (!DatabaseUtility
                .InitializeDatabase(cameraAnimDatabase,
                out cameraAnimDatabase))
                return false;

            if (!DatabaseUtility
                .InitializeDatabase(evidenceDatabase,
                out evidenceDatabase))
                return false;

            if (!base.DatabasesAreValid())
                return false;

            return true;
        }

        protected override void DisplayDialogueOptions()
        {
            base.DisplayDialogueOptions();

            if (typeof(Line).IsAssignableFrom(typeof(T)))
                DisplayConvertField(dia.GetLines<Line>());
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lines"></param>
        protected virtual void DisplayConvertField<T2>(List<T2> lines) where T2 : Line, new()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                ConvertDialogueField();
                
                if (lines != null)
                    DisplayConversionButton(lines);
                else
                    HandyFields.Label("ALERT: There was a problem getting the lines from this dialogue. Conversion will fail.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ConvertDialogueField()
        {
            HandyFields.Label("Convert Dialogue :");

            conversionDialogue = HandyFields.UnityField(conversionDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lines"></param>
        protected virtual void DisplayConversionButton<T2>(List<T2> lines) where T2 : Line, new()
        {
            if (conversionDialogue is IConvertableDialogue convertable &&
                    HandyFields.Button("Apply Conversion") && ConfirmConversion(convertable, lines))
                ConversionProcess(convertable, lines);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dialogue"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual bool ConfirmConversion<T2>(IConvertableDialogue dialogue, List<T2> target) where T2 : Line, new()
        {
            string message = $"Convert {dialogue.LineCount} lines from dialogue?\n\n";

            if (target.Count > dialogue.LineCount)
                message += $"WARNING: Target has {target.Count} lines. {target.Count - dialogue.LineCount} will be lost!";
            else if (target.Count > 0)
                message += $"This will overwrite {target.Count} existing lines.";

            message += "\n\nTHIS CANNOT BE UNDONE!";

            return EditorUtility.DisplayDialog("Convert", message, "Convert", "Cancel");
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dialogue"></param>
        /// <param name="lines"></param>
        protected virtual void ConversionProcess<T2>(IConvertableDialogue dialogue, List<T2> lines) where T2 : Line, new()
        {
            if (dialogue == null || dialogue.LineCount < 0 || lines == null)
                return;

            for (int i = 0; i < dialogue.LineCount; i++)
            {
                Line line = dialogue.GetLine(i);

                if (line == null)
                {
                    UnityEngine.Debug.LogWarning($"ALERT: Line index {i} was null during conversion.");

                    continue;
                }

                if (i >= lines.Count)
                    lines.Add(new());

                lines[i].SetSpeakerNum(line.SpeakerNumber);

                lines[i].SetText(line.Text);

                lines[i].SetExpressionNum(line.ExpressionNumber);

                lines[i].SetExpression(line.Expression);
            }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayStartingMusic()
        {
            music ??= serializedObject.FindProperty(MusicFieldName);

            using (new EditorGUILayout.HorizontalScope())
            {
                HandyFields.Label("Starting Music :");

                EditorGUIUtility.labelWidth = 1;

                EditorGUILayout.PropertyField(music);
            }
        }


        public virtual SerializedProperty GetPhraseProperty(object phrase)
        {
            iterator = serializedObject.GetIterator();

            iterator.NextVisible(true);

            try
            {
                while (iterator.NextVisible(true))
                    if (iterator.GetTargetObjectOfProperty() == phrase)
                        return iterator;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }

            return null;
        }
    }
}