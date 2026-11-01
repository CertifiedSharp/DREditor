//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Minigames.Debates;
using DREditor.Editor.Toolbox;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    
    // THIS NEEDS TO BE A NEW DREDITOR CORE INTERFACE CALLED IExpressionVerifier where Dialogue assets can handle the 
    // process for verifying the expression.
    // This class can then be deleted.


    public static class VerifyMinigames
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Verify/Panel Debates")]
        public static void VerifyPanelDebates() => BeginVerification<DebateBuilder<Panel>, Panel>();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T4"></typeparam>
        static void BeginVerification<T, T4>()
            where T : DebateBuilder<T4>
            where T4 : Line
        {
            var assets = VerifyDialogues.FindAssetsByType<T>();

            for (int j = 0; j < assets.Count; j++)
            {
                T dia = assets[j];

                bool changed = false;

                UnityEngine.Debug.Log("Scanning: " + dia.name);

                for (int i = 0; i < dia.Lines.Count; i++)
                {
                    var currentLine = dia.Lines[i];

                    var currentSpeaker = currentLine.Speaker;

                    // If sprite moved position on characters expression list
                    if (currentSpeaker != null &&
                        currentLine.ExpressionNumber != 0 &&
                        currentSpeaker.Expressions[currentLine.ExpressionNumber - 1].Name != currentLine.Expression.Name)
                    {
                        if (MaterialVerifiedExpression(dia, currentLine, i))
                        {
                            changed = true;

                            continue;
                        }

                        if (NameVerifiedExpression(dia, currentLine, i))
                            changed = true;
                    }
                }

                if (changed)
                    EditorUtility.SetDirty(dia);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dia"></param>
        /// <param name="currentLine"></param>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        static bool MaterialVerifiedExpression<T, T2>(T dia, T2 currentLine, int lineIndex)
            where T : ScriptableObject
            where T2 : Line
        {
            if (currentLine.Speaker.Expressions.Contains(currentLine.Expression))
            {
                currentLine.SetExpressionNum(currentLine.Speaker.Expressions.IndexOf(currentLine.Expression) + 1);

                UnityEngine.Debug.Log("By Expression, Changed " + dia.name + "'s Line " + lineIndex + " " +
                        currentLine.Speaker.FirstName + "'s expression number for " + currentLine.Expression.Name);

                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dia"></param>
        /// <param name="currentLine"></param>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        static bool NameVerifiedExpression<T, T2>(T dia, T2 currentLine, int lineIndex)
            where T : ScriptableObject
            where T2 : Line
        {
            Expression expression = currentLine.Speaker.GetExpressionMatching(currentLine.Expression.Name);

            currentLine.SetExpressionNum(currentLine.Speaker.Expressions.IndexOf(expression) + 1);

            UnityEngine.Debug.Log("By Name, Changed: " + dia.name + "'s Line " + lineIndex +
                " " + currentLine.Speaker.FirstName + "'s expression number for " + expression.Name);

            return expression != null;
        }
    }
}