//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class LineButtonEditor<T> where T : Line, new()
    {
        SerializedObject serializedObject; 

        int i; 

        List<T> Lines;

        public virtual void Display(SerializedObject serializedObject, int i, List<T> Lines)
        {
            Initialize(serializedObject, i, Lines);

            Display();

            Resolve();
        }

        protected virtual void Initialize(SerializedObject serializedObject, int i, List<T> Lines)
        {
            this.serializedObject = serializedObject;

            this.i = i;

            this.Lines = Lines;
        }

        protected virtual void Resolve()
        {
            serializedObject = null;

            Lines = null;
        }

        protected virtual void Display() =>
            CustomLayoutUtility.DisplayScoped(
                new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(25)),
                RemoveLine,
                MoveLine,
                DuplicateButton,
                AddLine);


        protected virtual void RemoveLine()
        {
            if (Lines.Count > 1 && GUILayout.Button(new GUIContent("-", "Delete this Line."), GUILayout.Width(20)))
            {
                GUI.FocusControl(null);
                Lines.Remove(Lines[i]);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

        protected virtual void MoveLine()
        {
            if (i > 0)
            {
                if (GUILayout.Button(new GUIContent("ʌ", "Move this line up."), GUILayout.Width(20)))
                {
                    GUI.FocusControl(null);
                    Lines.Swap(i, i - 1);
                }
            }
            else
                GUILayout.Space(20f);

            if (i < Lines.Count - 1)
            {
                if (GUILayout.Button(new GUIContent("v", "Move this line down."), GUILayout.Width(20)))
                {
                    GUI.FocusControl(null);
                    Lines.Swap(i, i + 1);
                }
            }
            else
                GUILayout.Space(20f);

            GUILayout.Space(25f);

            GUILayout.FlexibleSpace();
        }

        protected virtual void DuplicateButton()
        {
            if (GUILayout.Button(new GUIContent("*", "Duplicate this Line below."), GUILayout.Width(20)))
            {
                T Copy = new()
                {
                    TranslationKey = Lines[i].TranslationKey,
                    VoiceSFX = Lines[i].VoiceSFX,
                    TimeToNextLine = Lines[i].TimeToNextLine,
                    AutomaticLine = Lines[i].AutomaticLine,
                };

                foreach (var audio in Lines[i].SFXs)
                    Copy.SFXs.Add(audio);

                foreach (var sceneEvents in Lines[i].Events)
                    Copy.Events.Add(sceneEvents);

                Copy.SetSpeaker(Lines[i].Speaker);

                Copy.SetSpeakerNum(Lines[i].SpeakerNumber);

                Copy.SetExpression(Lines[i].Expression);

                Copy.SetExpressionNum(Lines[i].ExpressionNumber);

                Copy.SetText(Lines[i].Text);

                Copy.SetAliasNumber(Lines[i].AliasNumber);

                Copy.SetDiaEvents(new List<IDialogueEvent>());

                foreach (IDialogueEvent ev in Lines[i].DiaEvents)
                    Copy.DiaEvents.Add(ev);

                Lines.Insert(i + 1, Copy);

                serializedObject.Update();
            }
        }

        protected virtual void AddLine()
        {
            if (GUILayout.Button(new GUIContent("+", "Add a new Line below."), GUILayout.Width(20)))
            {
                Lines.Insert(i + 1, InsertNewLine());

                serializedObject.Update();
            }

            GUILayout.FlexibleSpace();
        }

        protected virtual T InsertNewLine()
        {
            T _neoLine = new();

            _neoLine.SetSpeaker(null);

            _neoLine.SetSpeakerNum(-1);

            _neoLine.SetDiaEvents(new List<IDialogueEvent>());

            return _neoLine;
        }
    }
}
#endif