//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Editor.Toolbox
{
    [Serializable]
    public class ExpressionCaptureGroup : IDialogueCaptureGroup, IValidCaptureGroup
    {
        [ShowIf(nameof(GroupValid))]
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected CharacterDatabase characterDatabase;


        public virtual bool IsPriority => false;

        public virtual bool GroupValid()
        {
            bool result = DatabaseUtility.InitializeDatabase(characterDatabase, out characterDatabase);

            if (!result)
                EditorUtility.DisplayDialog("CharacterDatabase was not specified.",
                "Please create and assign a CharacterDatabase to use: " + nameof(CharacterCaptureGroup), "OK");

            return result;
        }

        public virtual void Capture<T>(ref T line, string capturedText) where T : Line
        {
            line.SetExpression(FindExpression(capturedText, line.Speaker, out int expressionIndex));

            line.SetExpressionNum(expressionIndex);
        }

        protected Expression FindExpression(string exName, Character character, out int exNum)
        {
            for (int i = 0; i < characterDatabase.Characters.Count; i++)
            {
                if (characterDatabase.Characters[i].FirstName == character.FirstName)
                {
                    for (int en = 0; en < characterDatabase.Characters[i].Expressions.Count; en++)
                    {
                        if (exName == characterDatabase.Characters[i].Expressions[en].Name)
                        {
                            exNum = en + 1;

                            return characterDatabase.Characters[i].Expressions[en];
                        }
                    }
                }
            }

            exNum = 0;

            return null;
        }
    }
}