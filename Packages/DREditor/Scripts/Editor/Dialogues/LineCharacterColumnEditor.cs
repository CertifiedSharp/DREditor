//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class LineCharacterColumnEditor<T> where T : Line, new()
    {
        DialogueCore dia;

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
            this.i = i;

            this.Lines = Lines;

            dia = serializedObject.targetObject as DialogueCore;
        }

        protected virtual void Resolve()
        {
            dia = null;

            Lines = null;
        }

        protected virtual void Display()
        {
            CustomLayoutUtility.DisplayScoped(new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(130)),
                DisplayCharacter,
                DisplayAlias,
                DisplaySceneEvents,
                DisplayOptions);
        }

        protected virtual void DisplayCharacter()
        {
            var prependedArray = ContainerUtil.PrependedList(dia.GetCharacterNames(), "<No Character>");

            Lines[i].SetSpeakerNum(EditorGUILayout.IntPopup(Lines[i].SpeakerNumber, prependedArray,
                ContainerUtil.Iota(prependedArray.Length, -1), GUILayout.MaxWidth(130)));

            Lines[i].SetSpeaker(Lines[i].SpeakerNumber == -1 ? null : dia.Speakers.Characters[Lines[i].SpeakerNumber]);

            if (Lines[i].SpeakerNumber == -1)
            {
                Lines[i].SetSpeaker(null);
                Lines[i].VoiceSFX = null;
                Lines[i].SetExpression(null);
                Lines[i].SetExpressionNum(0);
                Lines[i].SetAliasNumber(0);
            }
            else
                Lines[i].SetSpeaker(dia.Speakers.Characters[Lines[i].SpeakerNumber]);
        }

        protected virtual void DisplayAlias()
        {
            if (Lines[i].Speaker == null || Lines[i].Speaker.Aliases.Count == 0)
                return;

            var aliasNames = new string[Lines[i].Speaker.Aliases.Count + 1];

            aliasNames[0] = "(No Alias)";

            for (int j = 1; j < Lines[i].Speaker.Aliases.Count + 1; j++)
                aliasNames[j] = Lines[i].Speaker.Aliases[j - 1].Name;

            Lines[i].SetAliasNumber(EditorGUILayout.IntPopup(Lines[i].AliasNumber,
                aliasNames, dia.GetIntValues(Lines[i].Speaker.Aliases.Count + 1),
                    GUILayout.MaxWidth(130)));
        }

        protected virtual void DisplaySceneEvents()
        {
            if (Lines[i].Events != null)
            {
                for (var j = 0; j < Lines[i].Events.Count; j++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        Lines[i].Events[j] = (SceneEvent)EditorGUILayout.ObjectField(
                            Lines[i].Events[j], typeof(SceneEvent), false, GUILayout.Width(100));

                        if (HandyFields.SkinnyButton("x", GUILayout.Width(20)))
                            Lines[i].Events.Remove(Lines[i].Events[j]);
                    }
                }
            }

            if (HandyFields.Button("Add Scene Event"))
                Lines[i].Events.Add(ScriptableObject.CreateInstance<SceneEvent>());
        }



        protected virtual void ToggleOptionsButton() =>
            Lines[i].ShowOptions = HandyFields.Button("Toggle Options") ? !Lines[i].ShowOptions : Lines[i].ShowOptions;

        protected virtual void DisplayOptions() =>
            CustomLayoutUtility.MassHorizontal(
                    ToggleOptionsButton,
                    DontPanOption,
                    PanToCharacterOption,
                    PanToCharacterDropdown,
                    DisplayAutoOption);

        protected virtual void DontPanOption()
        {
            if (!Lines[i].ShowOptions && !Lines[i].DontPan)
                return;

            Lines[i].DontPan = HandyFields.Option(Lines[i].DontPan, "Don't Pan: ");
        }

        protected virtual void PanToCharacterOption()
        {
            if (!Lines[i].ShowOptions && !Lines[i].PanToChar)
                return;

            Lines[i].PanToChar = HandyFields.Option(Lines[i].PanToChar, "Pan To Character: ");
        }

        protected virtual void PanToCharacterDropdown()
        {
            if (!Lines[i].PanToChar)
                return;

            var prependedArray = ContainerUtil.PrependedList(dia.GetCharacterNames(), "<No Character>");

            Lines[i].CharToPanNum = EditorGUILayout.IntPopup(Lines[i].CharToPanNum, prependedArray,
                ContainerUtil.Iota(prependedArray.Length, -1), GUILayout.Width(130));

            Lines[i].CharToPan = Lines[i].CharToPanNum == -1 ? null : dia.Speakers.Characters[Lines[i].CharToPanNum];
        }

        protected virtual void DisplayAutoOption()
        {
            if (!Lines[i].ShowOptions && !Lines[i].AutomaticLine)
                return;

            HandyFields.Label("Auto:");

            Lines[i].AutomaticLine = EditorGUILayout.Toggle(Lines[i].AutomaticLine, GUILayout.Width(15));

            if (Lines[i].AutomaticLine)
            {
                Lines[i].TimeToNextLine = EditorGUILayout.FloatField(Lines[i].TimeToNextLine, GUILayout.Width(24));

                EditorGUILayout.LabelField("sec", GUILayout.Width(22));
            }
        }
    }
}
#endif