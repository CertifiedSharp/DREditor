//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using DREditor.Dialogues;
using DREditor.EventObjects;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    [CustomClassEditor(typeof(Variable))]
    public class VariableEventEditor : VariableConditionEditor
    {
        /// <summary>
        /// 
        /// </summary>
        protected int value;

        /// <summary>
        /// 
        /// </summary>
        protected Color background;


        /// <inheritdoc/>
        protected override void DisplayVariableCondition()
        {
            if (target is not Variable Variable)
                return;

            background = GUI.backgroundColor;

            using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
            {
                DisplayBoolCondition(Variable);

                base.DisplayVariableCondition();

                DisplayVariableDialogue("True: ", ref Variable.NextDialogueTrue, ref Variable.NextIndexInDialogueTrue);

                DisplayVariableDialogue("False: ", ref Variable.NextDialogueFalse, ref Variable.NextIndexInDialogueFalse);

                GUI.backgroundColor = background;
            }
        }

        protected virtual void DisplayVariableDialogue(string label, ref DialogueCore dialogue, ref int index)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(80));

                if (dialogue == null)
                    GUI.backgroundColor = Color.cyan;

                dialogue = (DialogueCore)EditorGUILayout.ObjectField(
                    dialogue, typeof(DialogueCore), false, GUILayout.Width(200));

                GUI.backgroundColor = background;

                if (dialogue != null)
                {
                    string[] nextDialogueTexts = new string[dialogue.LineCount];

                    int[] optValues2 = new int[dialogue.LineCount];

                    for (int j = 0; j < dialogue.LineCount; j++)
                    {
                        nextDialogueTexts[j] = dialogue.GetLine(j).Text;

                        optValues2[j] = value;

                        value++;
                    }

                    index = EditorGUILayout.IntPopup(index, nextDialogueTexts, optValues2, GUILayout.Width(100));
                }
            }

            value = 0;
        }

        protected virtual void DisplayBoolCondition(Variable Variable)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = background;

                EditorGUILayout.LabelField("If ", GUILayout.Width(80));

                Variable.BoolVariable =
                    (BoolWithEvent)EditorGUILayout.ObjectField(
                        Variable.BoolVariable, typeof(BoolWithEvent), false, GUILayout.Width(200));
            }
        }
    }
}