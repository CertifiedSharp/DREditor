//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    [CustomClassEditor(typeof(ChoiceEvent))]
    public class ChoiceEventEditor : NestedClassEditorBase<IChoice>
    {
        /// <inheritdoc/>
        protected override string TargetFieldName => "choices";

        /// <inheritdoc/>
        protected override List<IChoice> Instances => ((ChoiceEvent)baseTarget).Choices;
    }

    [CustomClassEditor(typeof(Choice))]
    public class ChoiceEditor : ClassEditorTypeBase<Choice>
    {
        protected int value;

        protected Color backgroundColor;

        protected override void OnInspectorGUI()
        {
            backgroundColor = GUI.backgroundColor;

            using (new EditorGUILayout.HorizontalScope("Box"))
                DisplayMainChoice(target);

            DisplayLowerChoice(target);

            GUI.backgroundColor = backgroundColor;
        }

        protected virtual void DisplayMainChoice(Choice choice)
        {
            choice.ChoiceText = EditorGUILayout.TextField(
                choice.ChoiceText, GUILayout.Width(Screen.width - 343));

            if (choice.NextDialogue == null)
                GUI.backgroundColor = Color.cyan;

            choice.NextDialogue =
                (Dialogue)EditorGUILayout.ObjectField(choice.NextDialogue, typeof(Dialogue), true,
                    GUILayout.Width(100));

            GUI.backgroundColor = backgroundColor;

            if (choice.NextDialogue != null)
            {
                string[] nextDialogueTexts = new string[choice.NextDialogue.Lines.Count];

                int[] optValues = new int[choice.NextDialogue.Lines.Count];

                for (int j = 0; j < choice.NextDialogue.Lines.Count; j++)
                {
                    nextDialogueTexts[j] = choice.NextDialogue.Lines[j].Text;

                    optValues[j] = value;

                    value++;
                }

                value = 0;

                GUI.backgroundColor = Color.white;

                GUI.backgroundColor = choice.NextDialogue.Color;

                choice.NextIndexInDialogue = EditorGUILayout.IntPopup(
                    choice.NextIndexInDialogue, nextDialogueTexts, optValues, GUILayout.Width(100));
            }
        }

        protected virtual void DisplayLowerChoice(Choice choice)
        {
            choice.CustomCall = HandyFields.StringField("Custom Call: ", choice.CustomCall);

            using (new EditorGUILayout.HorizontalScope("Box"))
                choice.CallBreak = HandyFields.Option(choice.CallBreak, "Call Break: ", 70);

            GUILayout.Space(5);

            EditorGUILayout.LabelField("===========================");

            GUILayout.Space(5);
        }
    }
}
#endif