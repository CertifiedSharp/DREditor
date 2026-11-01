//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class DialogueDebugToolEditor<T> : DialogueDebugToolEditorBase where T : Line, new()
    {
        /// <summary>
        /// 
        /// </summary>
		protected DialogueCore dia;

        /// <summary>
        /// 
        /// </summary>
		protected bool exitDebugMode = false;


        /// <summary>
        /// 
        /// </summary>
        protected virtual void EnterDebugMode() => exitDebugMode = false;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ExitDebugMode() => exitDebugMode = true;


        public override bool DebugMode(DialogueCore dia)
        {
            EnterDebugMode();

            this.dia = dia;

            if (GUILayout.Button("Test First Line Characters Sprites", GUILayout.Width(250)))
                ReplaceWithFirstLineCharactersSprites();

            if (GUILayout.Button("Remove top half of Lines"))
                SplitDialogue(true);

            if (GUILayout.Button("Remove bottom half of Lines"))
                SplitDialogue(false);

            return exitDebugMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="autoAccept"></param>
        public virtual void ReplaceWithFirstLineCharactersSprites(DialogueCore dia = null, bool autoAccept = false)
        {
            if (dia == null)
                dia = this.dia;

            if (HasValidSpeakerOnFirstLine(dia) &&
                        (autoAccept || EditorUtility.DisplayDialog("Make dialogue have line of every sprite?",
                        "WARNING!: This will rewrite all lines of the dialogue asset! " +
                        "Are you sure you want to continue?", "Yes", "No")))
            {
                ReplaceLinesWithSprites(dia);

                ExitDebugMode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dia"></param>
        /// <returns></returns>
        protected virtual bool HasValidSpeakerOnFirstLine(DialogueCore dia)
        {
            bool result = dia.LineCount > 0 && dia.GetLine(0).Speaker != null && dia.GetLine(0).Speaker.Expressions.Count > 0;

            if (!result)
                Debug.LogWarning("ALERT: Please Add a line, add a Speaker to the first line, " +
                        "or add expressions the the character.");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dia"></param>
        protected virtual void ReplaceLinesWithSprites(DialogueCore dia)
        {
            Character character = dia.GetLine(0).Speaker;

            int speakerNum = dia.GetLine(0).SpeakerNumber;

            for (int i = dia.LineCount - 1; i > 0; i--) // Make it just one line
            {
                if (dia.LineCount == 1)
                    break;

                dia.GetLines<T>().Remove((T)dia.GetLine(i));
            }

            for (int i = 0; i < character.Expressions.Count; i++)
            {
                if (i == 0)
                {
                    dia.GetLine(i).SetExpressionNum(i + 1);

                    dia.GetLine(i).SetExpression(character.Expressions[i]);

                    dia.GetLine(i).SetText(dia.GetLine(i).Expression.Name);

                    continue;
                }

                dia.GetLines<T>().Add(new T());

                dia.GetLine(i).SetSpeakerNum(speakerNum);

                dia.GetLine(i).SetExpressionNum(i + 1);

                dia.GetLine(i).SetExpression(character.Expressions[i]);

                dia.GetLine(i).SetText(dia.GetLine(i).Expression.Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        protected virtual void SplitDialogue(bool top)
        {
            if (top)
                dia.GetLines<T>().RemoveRange(0, dia.LineCount / 2);
            else
                dia.GetLines<T>().RemoveRange(dia.LineCount / 2, dia.LineCount - dia.LineCount / 2);

            ExitDebugMode();
        }
    }
}
#endif