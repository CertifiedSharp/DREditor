//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using DREditor.Progressing;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public class EndEventEditor<T> where T : Line
	{
		protected DialogueBase<T> dia;

        protected int value;


        [SerializeField] protected VariableEventEditor<T> VariableEditor;

        [SerializeField] protected TriggerFlagEditor FlagEditor;
        protected readonly SceneTransitionEditor SceneEditor = new();


        public DialogueBase<T> GetDialogue() => dia;

        public virtual void DisplayEndEvents(DialogueBase<T> dia)
		{
            this.dia = dia == null ? ScriptableObject.CreateInstance<DialogueBase<T>>() : dia;

            if (VariableEditor == null || FlagEditor == null)
                Initialize();

            DisplayEvents();
        }

        protected virtual void Initialize()
        {
            DatabaseUtility.InitializeDatabase(null, out ProgressionDatabase progression);

            VariableEditor ??= new(progression);

            FlagEditor ??= new(progression);
        }


        protected virtual void DisplayEvents()
		{
            if (VariableEditor.Display(dia.Variable) is Variable<T> variable)
                dia.SetVariable(variable);

            if (dia.DirectTo.Enabled)
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Next Dialogue:", GUILayout.Width(90));

                        if (dia.DirectTo.NewDialogue == null)
                        {
                            GUI.backgroundColor = Color.cyan;
                        }

                        dia.DirectTo.SetNewDialogue(
                            (DialogueBase<T>)EditorGUILayout.ObjectField(dia.DirectTo.NewDialogue, typeof(DialogueBase<T>), false,
                                GUILayout.Width(180)));

                        GUI.backgroundColor = dia.Color;

                        if (dia.DirectTo.NewDialogue != null)
                        {
                            string[] nextDialogueTexts = new string[dia.DirectTo.NewDialogue.Lines.Count];

                            int[] optValues = new int[dia.DirectTo.NewDialogue.Lines.Count];

                            for (int j = 0; j < dia.DirectTo.NewDialogue.Lines.Count; j++)
                            {
                                nextDialogueTexts[j] = dia.DirectTo.NewDialogue.Lines[j].Text;
                                optValues[j] = value;
                                value++;
                            }

                            dia.DirectTo.SetNewDialogueIndex(EditorGUILayout.IntPopup(
                                dia.DirectTo.NewDialogueIndex, nextDialogueTexts, optValues, GUILayout.Width(100)));
                        }

                    }
                    value = 0;

                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        dia.DirectTo.SetNewDialogue(null);

                        dia.DirectTo.SetEnabled(false);
                    }
                }
            }

            dia.SetSceneTransition(SceneEditor.Display(dia.SceneTransition));

            dia.SetTriggerFlag(FlagEditor.Display(dia.FlagTrigger));
        }

        public virtual void DisplayEndEventsButtons()
		{
            if (dia.LineCount != 0)
                CustomLayoutUtility.MassHorizontal(DisplayVariableButtons, DisplayButtons);
        }

		protected virtual void DisplayVariableButtons()
		{
            VariableEditor.DisplayAddButton();

            if (!dia.Variable.Enabled && !dia.DirectTo.Enabled && GUILayout.Button("Direct to...", GUILayout.Width(100)))
                dia.DirectTo.SetEnabled(true);
        }

        protected virtual void DisplayButtons()
        {
            SceneEditor.DisplayAddButton();

            FlagEditor.DisplayAddButton();
        }
	}
}
#endif