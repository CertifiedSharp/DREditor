//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public class DialogueEndEventEditor : EndEventEditor<Line>
    {
        protected Dialogue dialogue;

        protected readonly CustomCallEndEventEditor CustomCallEditor = new();

        public DialogueEndEventEditor() : base() { }

        public override void DisplayEndEvents(DialogueBase<Line> dia)
        {
            if (dia is Dialogue dialogueType)
                dialogue = dialogueType;

            base.DisplayEndEvents(dia);
        }

        protected override void DisplayEvents()
        {
            DisplayChoices();

            dialogue.SetEndEvent(CustomCallEditor.Display(dialogue.EndEvent));

            DisplayInvestigation();

            base.DisplayEvents();
        }

        protected virtual void DisplayChoices()
        {
            if (dialogue.Choices == null)
                return;

            for (int i = 0; i < dialogue.Choices.Count; i++)
            {
                GUILayout.Label("Choice " + (i + 1).ToString(), GUILayout.Width(80));

                using (new EditorGUILayout.HorizontalScope("Box"))
                    DisplayMainChoice(dialogue.Choices[i]);

                DisplayLowerChoice(dialogue.Choices[i]);
            }

            GUI.backgroundColor = dialogue.Color;

            if (dialogue.Choices.Count > 0 && GUILayout.Button("Remove Choices", GUILayout.Width(130)))
                dialogue.Choices.Clear();
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

            GUI.backgroundColor = dialogue.Color;

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

        protected virtual void DisplayInvestigation()
        {
            if (dialogue.StartInvestigation.Enabled)
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        dialogue.StartInvestigation.NextObjective =
                            HandyFields.Option(dialogue.StartInvestigation.NextObjective, "Next Objective: ", 100);

                        dialogue.StartInvestigation.NewDialogue =
                            HandyFields.UnityField(dialogue.StartInvestigation.NewDialogue, 120, 30);

                        dialogue.StartInvestigation.ID =
                            HandyFields.IntField("ID: ", dialogue.StartInvestigation.ID, 100);
                    }

                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        dialogue.StartInvestigation.NextObjective = false;
                        dialogue.StartInvestigation.SetEnabled(false);
                        dialogue.StartInvestigation.NewDialogue = null;
                    }
                }
            }
        }

        protected override void DisplayButtons()
        {
            if (dialogue.Choices.Count != 0)
                return;

            base.DisplayButtons();

            if (!dialogue.StartInvestigation.Enabled && GUILayout.Button("End Event", GUILayout.Width(100)))
                dialogue.StartInvestigation.SetEnabled(true);
        }

        protected override void DisplayVariableButtons()
        {
            if (dialogue.Choices.Count == 0)
            {
                base.DisplayVariableButtons();

                CustomCallEditor.DisplayAddButton();

                if (GUILayout.Button("Add Choice", GUILayout.Width(100)))
                {
                    dialogue.Choices.Add(new Choice());
                    dialogue.Choices.Add(new Choice());
                    dialogue.Choices.Add(new Choice());
                }
            }
        }
    }
}
#endif