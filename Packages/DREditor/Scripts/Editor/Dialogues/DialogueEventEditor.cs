//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueEventEditor : InterfaceEditor<IDialogueEvent>
    {
        public DialogueEventEditor() : base() { }

        public DialogueEventEditor(UnityEditor.Editor parentEditor, List<IDialogueEvent> diaEvents,
            bool showDiaEvents, int currentlyPickedIndex = -1) : 
            base(parentEditor, diaEvents, showDiaEvents, currentlyPickedIndex) { }

        protected override void DisplayValidInterfaceInstance()
        {
            base.DisplayValidInterfaceInstance();

            if (instances[currentlyDisplayedEventIndex] is IDisplayableDialogueEvent dialogueEvent)
            {
                dialogueEvent?.EditorUI(GetPastProperty());

                GUILayout.Space(5);

                dialogueEvent?.ShowHelpBox();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual object GetPastProperty() => eventListProperty == null ?
                instances[currentlyDisplayedEventIndex] :
                eventListProperty.GetArrayElementAtIndex(currentlyDisplayedEventIndex);

        protected override bool HasCustomEditorProcess() => instances[currentlyDisplayedEventIndex] is IDisplayableDialogueEvent;

        protected override bool RemoveInterfaceInstanceButton()
        {
            bool result = base.RemoveInterfaceInstanceButton();

            if (!result && instances[currentlyDisplayedEventIndex] is IDisplayableDialogueEvent dialogueEvent && HandyFields.Button("?", "Show Help"))
                dialogueEvent.ToggleHelpBox();

            return result;
        }
    }
}