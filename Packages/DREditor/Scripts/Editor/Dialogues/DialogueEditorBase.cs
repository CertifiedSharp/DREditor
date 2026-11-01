//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public abstract class DialogueEditorBase<T> : UnityEditor.Editor where T : new()//where T : Line, new()
    {
        protected bool editNameFoldout = false;

        protected bool debugMode;

        protected DialogueBase<T> dia;

        protected abstract LineEditorBase LineEditor { get; }

        protected abstract DialogueDebugToolEditorBase DebugTool { get; }

        protected virtual void SetDialogue(DialogueBase<T> dia) => this.dia = dia;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CreateForm();

            EditorUtility.SetDirty(dia);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void CreateForm()
        {
            if (!DatabasesAreValid() || DisplayDebugTool())
                return;

            DisplayDialogueOptions();

            SetWordWrappingColor();

            EditPanels();

            EditFooter();
        }

        protected virtual bool DatabasesAreValid()
        {
            if (DatabaseUtility.InitializeDatabase(dia.Speakers, out CharacterDatabase speakers) && speakers.IsValid())
                dia.SetSpeakers(speakers);
            else
                return false;

            return true;
        }

        protected virtual bool DisplayDebugTool()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                debugMode = HandyFields.Option(debugMode, "Debug Mode: ", 80);

                EditorGUILayout.LabelField(dia.DialogueName, EditorStyles.boldLabel);

                dia.SetColor(EditorGUILayout.ColorField(dia.Color, GUILayout.Width(50)));
            }

            if (debugMode)
            {
                GUI.enabled = false;

                EditDialogueNameFoldout();

                GUI.enabled = true;
            }

            if (debugMode && DebugTool.DebugMode(dia))
                debugMode = false;

            return debugMode;
        }

        protected virtual void SetWordWrappingColor()
        {
            EditorStyles.textArea.wordWrap = true;

            EditorStyles.textField.wordWrap = true;

            GUI.backgroundColor = dia.Color;
        }


        protected virtual void EditDialogueNameFoldout()
        {
            editNameFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(editNameFoldout, "Edit DialogueName");

            if (editNameFoldout)
                EditDialogueName();

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        protected virtual void EditDialogueName()
        {

            using (new EditorGUILayout.VerticalScope("Box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (dia.DialogueName == "")
                        dia.SetName(target.name);

                    dia.SetName(EditorGUILayout.DelayedTextField("Dialogue Code: ", dia.DialogueName));

                    if (GUILayout.Button("Update", GUILayout.Width(80)))
                    {
                        var path = AssetDatabase.GetAssetPath(dia);
                        var flds = path.Split('/');
                        var nm = flds[^2];

                        string tx = "";
                        if (dia.Lines.Count > 0)
                        {
                            tx = dia.GetLine(0).Text[..(dia.GetLine(0).Text.Length > 29 ? 30 : dia.GetLine(0).Text.Length)];
                            tx = tx.Replace('?', ' ');
                        }

                        string fileName = nm + "_" + dia.DialogueName + "_" + tx;

                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(dia), fileName);
                        UnityEngine.Debug.Log(fileName);
                        UnityEngine.Debug.Log(AssetDatabase.GetAssetPath(dia));
                    }
                }

                GUI.backgroundColor = Color.white;
            }
        }

        protected virtual void DisplayDialogueOptions()
        {
            if (dia is IInstantAbleDialogue instantableDialogue)
                using (new EditorGUILayout.HorizontalScope())
                    instantableDialogue.SetInstant(HandyFields.Option(instantableDialogue.GetInstant(), "Is Instant: ", 60));
        }

        protected virtual void EditPanels()
        {
            LineEditor.EditTestLine(dia.LineCount);

            if (dia.Lines == null)
                return;

            if (LineEditor.TestLine != -1)
                EditLine(LineEditor.TestLine);
            else
                for (int i = 0; i < dia.LineCount; i++)
                    EditLine(i);
        }

        protected virtual void EditLine(int i) => LineEditor.EditLine(i);

        protected virtual void EditFooter()
        {
            if (LineEditor.TestLine == -1)
                DisplayFooter();

            LineEditor.DrawHoverSprite();
        }

        protected virtual void DisplayFooter() => NewLineButton();

        protected virtual void NewLineButton()
        {
            using (new EditorGUILayout.HorizontalScope())
                if (GUILayout.Button("New " + ObjectNames.NicifyVariableName(typeof(T).Name), GUILayout.Width(100)))
                    dia.Lines.Add(new T());
        }
    }
}
#endif