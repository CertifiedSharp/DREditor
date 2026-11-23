//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using DREditor.Progressing;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public class EndEventEditor<T> where T : Line
	{
		protected DialogueBase<T> dia;

        protected int value;


        [SerializeField] protected VariableEventEditor VariableEditor;

        [SerializeField] protected TriggerFlagEditor FlagEditor;
        protected readonly SceneTransitionEditor SceneEditor = new();


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
            if (VariableEditor.Display(dia.Variable) is Variable variable)
                dia.SetVariable(variable);

            if (dia.DirectTo.Enabled)
                DisplayDirectTo();

            dia.SetSceneTransition(SceneEditor.Display(dia.SceneTransition));

            dia.SetTriggerFlag(FlagEditor.Display(dia.FlagTrigger));
        }

        protected virtual void DisplayDirectTo()
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
                        (DialogueCore)EditorGUILayout.ObjectField(dia.DirectTo.NewDialogue, typeof(DialogueCore), false,
                            GUILayout.Width(180)));

                    GUI.backgroundColor = dia.Color;

                    if (dia.DirectTo.NewDialogue != null)
                    {
                        string[] nextDialogueTexts = new string[dia.DirectTo.NewDialogue.LineCount];

                        int[] optValues = new int[dia.DirectTo.NewDialogue.LineCount];

                        for (int j = 0; j < dia.DirectTo.NewDialogue.LineCount; j++)
                        {
                            nextDialogueTexts[j] = dia.DirectTo.NewDialogue.GetLine(j).Text;
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

    [Serializable]
    public class EndEventDisplayer : InterfaceEditor<IEndEvent>
    {
        public EndEventDisplayer() : base() { }

        public EndEventDisplayer(UnityEditor.Editor parentEditor, List<IEndEvent> diaEvents,
            bool showDiaEvents, int currentlyPickedIndex = -1) :
            base(parentEditor, diaEvents, showDiaEvents, currentlyPickedIndex)
        { }

        //protected override void DisplayValidInterfaceInstance()
        //{
        //    base.DisplayValidInterfaceInstance();

        //    if (instances[currentlyDisplayedEventIndex] is IDisplayableDialogueEvent dialogueEvent)
        //    {
        //        dialogueEvent?.EditorUI(GetPastProperty());

        //        GUILayout.Space(5);

        //        dialogueEvent?.ShowHelpBox();
        //    }
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <returns></returns>
        //protected virtual object GetPastProperty() => eventListProperty == null ?
        //        instances[currentlyDisplayedEventIndex] :
        //        eventListProperty.GetArrayElementAtIndex(currentlyDisplayedEventIndex);

        //protected override bool HasCustomEditorProcess() => instances[currentlyDisplayedEventIndex] is IDisplayableDialogueEvent;

        //protected override bool RemoveInterfaceInstanceButton()
        //{
        //    bool result = base.RemoveInterfaceInstanceButton();

        //    if (!result && instances[currentlyDisplayedEventIndex] is IDisplayableDialogueEvent dialogueEvent && HandyFields.Button("?", "Show Help"))
        //        dialogueEvent.ToggleHelpBox();

        //    return result;
        //}
    }
}
#endif