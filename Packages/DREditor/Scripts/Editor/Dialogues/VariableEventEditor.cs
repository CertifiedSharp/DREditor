//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Progressing;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class VariableEventEditor<T> : VariableConditionEditor where T : Line
    {
        protected int value;
        Color Color;
        public VariableEventEditor(ProgressionDatabase progression = null) : base(progression) { }

        protected override void DisplayVariableCondition()
        {
            if (variableBase is not Variable<T> Variable || !Variable.Enabled)
                return;

            Color = GUI.backgroundColor;

            using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
            {
                DisplayBoolCondition(Variable);

                base.DisplayVariableCondition();

                DisplayVariableDialogue("True: ", ref Variable.NextDialogueTrue, ref Variable.NextIndexInDialogueTrue);

                DisplayVariableDialogue("False: ", ref Variable.NextDialogueFalse, ref Variable.NextIndexInDialogueFalse);

                GUI.backgroundColor = Color;

                DisplayRemoveButton(Variable);
            }
        }

        protected virtual void DisplayVariableDialogue(string label, ref DialogueBase<T> dialogue, ref int index)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(80));

                if (dialogue == null)
                    GUI.backgroundColor = Color.cyan;

                dialogue = (DialogueBase<T>)EditorGUILayout.ObjectField(
                    dialogue, typeof(DialogueBase<T>), false, GUILayout.Width(200));

                GUI.backgroundColor = Color;

                if (dialogue != null)
                {
                    string[] nextDialogueTexts = new string[dialogue.Lines.Count];

                    int[] optValues2 = new int[dialogue.Lines.Count];

                    for (int j = 0; j < dialogue.Lines.Count; j++)
                    {
                        nextDialogueTexts[j] = dialogue.Lines[j].Text;

                        optValues2[j] = value;

                        value++;
                    }

                    index = EditorGUILayout.IntPopup(index, nextDialogueTexts, optValues2, GUILayout.Width(100));
                }
            }

            value = 0;
        }

        protected virtual void DisplayBoolCondition(Variable<T> Variable)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = Color;

                EditorGUILayout.LabelField("If ", GUILayout.Width(80));

                Variable.BoolVariable =
                    (BoolWithEvent)EditorGUILayout.ObjectField(
                        Variable.BoolVariable, typeof(BoolWithEvent), false, GUILayout.Width(200));
            }
        }

        protected virtual void DisplayRemoveButton(Variable<T> Variable)
        {
            if (GUILayout.Button("Remove Condition", GUILayout.Width(130)))
            {
                Variable.BoolVariable = null;

                Variable.SetEnabled(false);
            }
        }
    }
}