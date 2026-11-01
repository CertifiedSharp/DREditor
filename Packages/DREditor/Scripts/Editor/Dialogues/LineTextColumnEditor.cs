//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class LineTextColumnEditor<T> where T : Line, new()
    {
        SerializedProperty propLines;

        int i;

        List<T> Lines;

        public virtual void Display(SerializedProperty propLines, int i, List<T> Lines)
        {
            Initialize(propLines, i, Lines);

            Display();

            Resolve();
        }

        protected virtual void Initialize(SerializedProperty propLines, int i, List<T> Lines)
        {
            this.propLines = propLines;

            this.i = i;

            this.Lines = Lines;
        }

        protected virtual void Resolve()
        {
            propLines = null;

            Lines = null;
        }

        protected virtual void Display() => CustomLayoutUtility.DisplayVertical(DisplayLineText, DisplayNonSFXAudio);

        protected virtual void DisplayLineText() =>
            HandyFields.PropertyTextArea(propLines.GetArrayElementAtIndex(i)?.FindPropertyRelative("text"),
                GUILayout.MinHeight(62));

        protected virtual void DisplayNonSFXAudio()
        {
            HandyFields.FMODField("VO:", propLines.GetArrayElementAtIndex(i)?.FindPropertyRelative("voiceSFX"));

            if (Lines[i].ShowOptions ||
                !string.IsNullOrEmpty(propLines.GetArrayElementAtIndex(i)?.FindPropertyRelative("envSFX").stringValue))
                HandyFields.FMODField("Env:", propLines.GetArrayElementAtIndex(i)?.FindPropertyRelative("envSFX"));

            if (Lines[i].MusicChange)
                HandyFields.FMODField("Music:", propLines.GetArrayElementAtIndex(i)?.FindPropertyRelative("music"));
        }
    }
}
#endif