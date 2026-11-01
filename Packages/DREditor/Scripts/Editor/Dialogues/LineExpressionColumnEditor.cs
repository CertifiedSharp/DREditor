//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class LineExpressionColumnEditor<T> where T : Line, new()
    {
        DialogueCore dia;
        protected readonly ExpressionPreviewer expressionPreviewer = new();

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
            Lines = null;

            dia = null;
        }

        protected virtual void Display()
        {
            LineBase currLine = Lines[i];

            if (currLine.Speaker != null)
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.MaxWidth(100)))
                {
                    var exprs = currLine.Speaker.Expressions.Count;

                    if (exprs < currLine.ExpressionNumber)
                    {
                        currLine.SetExpressionNum(0);
                    }

                    if (currLine.Expression != null)
                    {
                        GUIStyle expr = new();
                        if (currLine.ExpressionNumber > 0)
                        {
                            var tex = HandyFields.GetMaterialTexture(currLine.Expression.Sprite);
                            if (tex)
                            {
                                expr.normal.background = tex;
                            }
                        }
                        EditorGUILayout.LabelField(GUIContent.none, expr, GUILayout.Width(100),
                            GUILayout.Height(100));

                        expressionPreviewer.SetExpression(currLine.Expression);
                    }

                    var expressionNames = new string[currLine.Speaker.Expressions.Count + 1];
                    expressionNames[0] = "<No change>";

                    for (int j = 1; j < currLine.Speaker.Expressions.Count + 1; j++)
                    {
                        expressionNames[j] = currLine.Speaker.Expressions[j - 1].Name;
                    }

                    currLine.SetExpressionNum(EditorGUILayout.IntPopup(currLine.ExpressionNumber,
                        expressionNames, dia.GetIntValues(currLine.Speaker.Expressions.Count + 1), GUILayout.Width(100)));

                    if (currLine.ExpressionNumber > 0)
                    {
                        currLine.SetExpression(currLine.Speaker.Expressions[currLine.ExpressionNumber - 1]);
                    }
                    else
                    {
                        currLine.SetExpression(new Expression());
                    }

                    CustomLayoutUtility.MassHorizontal(StopSFXOption, StopEnvOption, ChangeMusicOption);
                }
            }
            else
            {
                EditorGUILayout.LabelField(GUIContent.none, GUILayout.Width(108),
                    GUILayout.Height(100));
            }
        }

        public virtual void DrawHoveredSprite() => expressionPreviewer.DrawHoveredSprite();

        protected virtual void StopSFXOption() => Lines[i].StopSFX = DisplayOption(Lines[i].StopSFX, "Stop SFX:");

        protected virtual void StopEnvOption() => Lines[i].StopEnv = DisplayOption(Lines[i].StopEnv, "Stop Env:");

        protected virtual void ChangeMusicOption() => Lines[i].MusicChange = DisplayOption(Lines[i].MusicChange, "Change Music:");

        protected virtual bool DisplayOption(bool option, string label)
        {
            if (Lines[i].ShowOptions || option)
                option = HandyFields.Option(option, label);

            return option;
        }
    }
}
#endif