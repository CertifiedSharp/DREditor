//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// Custom Class editor for the choice option <see cref="Choice"/>.
    /// </summary>
    [CustomClassEditor(typeof(Choice))]
    public class ChoiceEditor : ClassEditorTypeBase<Choice>
    {
        /// <summary>
        /// The local index to track the selected line to move to on the dialogue when this <see cref="Choice"/> is selected.
        /// </summary>
        protected int linePopupIndex;

        /// <summary>
        /// The local cache to keep track and store the current GUI.backgroundColor.
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
        /// Displays the Choice Text and Dialogue to move to once the <see cref="Choice"/> is selected.
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
        /// Displays the index for the pop up for what line to move to within the <see cref="Choice"/>.
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

                    optValues[j] = linePopupIndex;

                    linePopupIndex++;
                }

                linePopupIndex = 0;

                GUI.backgroundColor = Color.white;

                target.NextIndexInDialogue = EditorGUILayout.IntPopup(
                    target.NextIndexInDialogue, nextDialogueTexts, optValues, GUILayout.Width(100));
            }
        }


        /// <summary>
        /// Displays the Custom Call Data within the <see cref="Choice"/>.
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