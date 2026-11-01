//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public class LineLowerEditor<T> where T : Line, new()
    {
        protected UnityEditor.Editor parentEditor;

        protected readonly DialogueEventEditor dialogueEventEditor = new();

        protected int diaEventIndex;

        SerializedProperty propLines;

        int i;

        List<T> Lines;

        public virtual void Display(SerializedProperty propLines, int i, List<T> Lines, UnityEditor.Editor parentEditor)
        {
            Initialize(propLines, i, Lines, parentEditor);

            Display();

            Resolve();
        }

        protected virtual void Initialize(SerializedProperty propLines, int i, List<T> Lines, UnityEditor.Editor parentEditor)
        {
            this.propLines = propLines;

            this.i = i;

            this.Lines = Lines;

            this.parentEditor = parentEditor;
        }

        protected virtual void Resolve()
        {
            propLines = null;

            Lines = null;
        }

        protected virtual void Display()
        {
            if (SkipDisplayingLowerLine())
                return;

            CustomLayoutUtility.DisplayVertical(DisplaySFX);

            DisplayDialogueEvents();
        }

        protected virtual bool SkipDisplayingLowerLine() => propLines == null || i >= propLines.arraySize || i >= Lines.Count;

        protected virtual void DisplaySFX()
        {
            if (Lines[i].SFXs != null)
                HandyFields.HardPropertyField("SFXs:", propLines.GetArrayElementAtIndex(i).FindPropertyRelative("SFXs"));
        }


        protected virtual void DisplayDialogueEvents()
        {
            T currentLine = Lines[i];

            dialogueEventEditor.Initialize(parentEditor, currentLine.DiaEvents, currentLine.ShowDiaEvents, diaEventIndex);

            var eventListProperty = propLines.GetArrayElementAtIndex(i).FindPropertyRelative("DiaEvents");

            dialogueEventEditor.DisplayInterfaceInstances(eventListProperty);

            currentLine.ShowDiaEvents = dialogueEventEditor.showDiaEvents;

            currentLine.SetDiaEvents(dialogueEventEditor.instances);

            diaEventIndex = dialogueEventEditor.currentlyPickedIndex;
        }
    }
}
#endif