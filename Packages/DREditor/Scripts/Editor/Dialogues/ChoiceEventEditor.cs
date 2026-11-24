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
    /// The filled out Editor for the End Event POCO class <see cref="ChoiceEvent"/>. <br/>
    /// Displays a dynamic list of <see cref="IChoice"/>.
    /// </summary>
    [CustomClassEditor(typeof(ChoiceEvent))]
    public class ChoiceEventEditor : NestedClassEditorBase<IChoice>
    {
        /// <inheritdoc/>
        protected override string TargetFieldName => "choices";

        /// <inheritdoc/>
        protected override List<IChoice> Instances => ((ChoiceEvent)baseTarget).Choices;
    }

    /// <summary>
    /// Custom Class editor for the choice option <see cref="Choice"/>.
    /// </summary>
    [CustomClassEditor(typeof(Choice))]
    public class ChoiceEditor : ClassEditorTypeBase<Choice>
    {
        /// <summary>
        /// 
        /// </summary>
        protected int value;

        /// <summary>
        /// 
        /// </summary>
        protected Color backgroundColor;


        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            backgroundColor = GUI.backgroundColor;

            CustomLayoutUtility.DisplayHorizontal("Box", DisplayMainChoice, DisplayToLineIndex);

            DisplayChoiceCustomCall();

            GUI.backgroundColor = backgroundColor;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayMainChoice()
        {
            target.ChoiceText = EditorGUILayout.TextField(
                target.ChoiceText, GUILayout.Width(Screen.width - 343));

            if (target.NextDialogue == null)
                GUI.backgroundColor = Color.cyan;

            target.NextDialogue =
                (Dialogue)EditorGUILayout.ObjectField(target.NextDialogue, typeof(Dialogue), true,
                    GUILayout.Width(100));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayToLineIndex()
        {
            GUI.backgroundColor = backgroundColor;

            if (target.NextDialogue != null)
            {
                string[] nextDialogueTexts = new string[target.NextDialogue.Lines.Count];

                int[] optValues = new int[target.NextDialogue.Lines.Count];

                for (int j = 0; j < target.NextDialogue.Lines.Count; j++)
                {
                    nextDialogueTexts[j] = target.NextDialogue.Lines[j].Text;

                    optValues[j] = value;

                    value++;
                }

                value = 0;

                GUI.backgroundColor = Color.white;

                GUI.backgroundColor = target.NextDialogue.Color;

                target.NextIndexInDialogue = EditorGUILayout.IntPopup(
                    target.NextIndexInDialogue, nextDialogueTexts, optValues, GUILayout.Width(100));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayChoiceCustomCall()
        {
            target.CustomCall = HandyFields.StringField("Custom Call: ", target.CustomCall);

            using (new EditorGUILayout.HorizontalScope("Box"))
                target.CallBreak = HandyFields.Option(target.CallBreak, "Call Break: ", 70);

            GUILayout.Space(5);

            EditorGUILayout.LabelField("===========================");

            GUILayout.Space(5);
        }
    }
}
#endif