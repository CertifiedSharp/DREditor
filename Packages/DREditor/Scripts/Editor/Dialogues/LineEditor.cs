//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public class LineEditor<T> : LineEditorBase where T : Line, new()
    {
        

        protected SerializedObject serializedObject;

        protected SerializedProperty propLines;

        protected UnityEditor.Editor parentEditor;

        protected readonly LineCharacterColumnEditor<T> characterColumnEditor = new();

        protected readonly LineExpressionColumnEditor<T> expressionColumnEditor = new();

        protected readonly LineTextColumnEditor<T> textColumnEditor = new();


        protected readonly LineButtonEditor<T> buttonEditor = new();
        protected readonly LineLowerEditor<T> lineLowerEditor = new();


        protected List<T> Lines;

        protected DialogueCore dia;


        

        public virtual void Initialize(DialogueCore dia, SerializedObject serializedObject = null, UnityEditor.Editor parentEditor = null,
            SerializedProperty linesProperty = null, List<T> lines = null)
        {
            this.dia = dia;

            this.serializedObject = serializedObject;

            this.parentEditor = parentEditor;

            Lines = lines ?? dia.GetLines<T>();

            propLines = serializedObject != null ? linesProperty ?? serializedObject.FindProperty("lines") : null;
        }

        public override void DrawHoverSprite() => expressionColumnEditor.DrawHoveredSprite();

        protected override void DisplayDialogueLine()
        {
            CustomLayoutUtility.DisplayScoped(new EditorGUILayout.HorizontalScope("Box"),
                DisplayColoredLine);

            ResolveColoredLine();
        }


        protected virtual void DisplayColoredLine() => CustomLayoutUtility.DisplayVertical(SetLineBGColor, DisplayLine);

        protected override void DisplayLineNumberSplitter() => HandySplitters.LineNumSplitter(dia.Color, 1, 125);

        protected virtual void ResolveColoredLine()
        {
            GUI.color = Color.white;

            HandySplitters.UISplitterCustom(dia.Color);
        }

        protected virtual void SetLineBGColor()
        {
            if (Lines[i].Speaker is not Student student)
                return;

            GUI.backgroundColor = Color.white;

            Color.RGBToHSV(student.StudentCard.Color, out float H, out float S, out float _);

            Color color = Color.HSVToRGB(H, Mathf.Min(S, 0.35f), 1f);

            GUI.color = color;
        }


        protected virtual void DisplayLine()
        {
            CustomLayoutUtility.DisplayScoped(
                new EditorGUILayout.HorizontalScope(GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth)),
                DisplayCharacterPanel,
                DisplayExpressionPanel,
                DisplayDialoguePanel,
                DisplayLinePositionPanel);

            GUILayout.Space(5f);

            DisplayLowerLine();
        }

        //Column 1
        protected virtual void DisplayCharacterPanel() =>
            CustomLayoutUtility.DisplayScoped(new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(130)),
                DisplayLineNumberBar,
                DisplayCharacterColumn);

        protected override void OnTestButtonPressed()
        {
            if (Application.isPlaying)
                DialogueCore.InvokeDebugLine(dia, Lines[i]);
            else
                EditorUtility.DisplayDialog("Must be in Play Mode", "You must be in play mode to test lines!", "Ok");
        }




        protected virtual void DisplayCharacterColumn() => characterColumnEditor.Display(serializedObject, i, Lines);


        protected virtual void DisplayExpressionPanel() => DisplayExpressionColumn();

        protected virtual void DisplayExpressionColumn() => expressionColumnEditor.Display(serializedObject, i, Lines);

        protected virtual void DisplayDialoguePanel()
        {
            if (!SkipDisplayingText())
                DisplayTextColumn();
        }

        protected virtual bool SkipDisplayingText() => i >= propLines.arraySize;

        protected virtual void DisplayTextColumn() => textColumnEditor.Display(propLines, i, Lines);

        protected virtual void DisplayLinePositionPanel() => buttonEditor.Display(serializedObject, i, Lines);


        protected virtual void DisplayLowerLine() => lineLowerEditor.Display(propLines, i, Lines, parentEditor);
    }
}
#endif