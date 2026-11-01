//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    public class DialogueBase<T> : DialogueCore
    {

        [FormerlySerializedAs("Lines")]
        public List<T> lines = new();
        public List<T> Lines { get => lines; }
        

        public SceneTransition SceneTransition = new();
        
        public TriggerFlag FlagTrigger = new(); //*

        public override int LineCount { get => Lines.Count; }

        public override void InvokeDialogue(ITypeCallback callback)
        {
        }
        
        public void SetTranslationKey(string translationKey) => this.translationKey = translationKey;

        public void SetColor(Color Color) => this.Color = Color;

        

        public override Line GetLine(int index)
        {
            int lineIndex = 0;

            if (index < Lines.Count && index >= 0)
                lineIndex = index;
            else
                UnityEngine.Debug.LogError($"ALERT: The line at the requested index ({index}) is out of bounds, defaulting to 0.");

            if (Lines[lineIndex] is Line castedLine)
                return castedLine;
            else
            {
                UnityEngine.Debug.LogWarning("ALERT: The line retrieved within the dialogue asset was not of type " + nameof(Line));

                return null;
            }
        }

        public override List<T2> GetLines<T2>()
        {
            if (Lines is List<T2> castedLines)
                return castedLines;

            if (Lines != null && typeof(T2).IsAssignableFrom(typeof(T)))
                return Lines.Cast<T2>().ToList();

            return null;
        }

        public override void AddNewLine()
        {
            if (typeof(T).IsAssignableFrom(typeof(Line)))
                Lines.Add(Activator.CreateInstance<T>());
        }

        public Variable<T> Variable = new();
        public DirectTo<T> DirectTo;

        public void SetVariable(Variable<T> Variable) => this.Variable = Variable;
        public void SetDirectTo(DirectTo<T> DirectTo) => this.DirectTo = DirectTo;
        public void SetSceneTransition(SceneTransition SceneTransition) => this.SceneTransition = SceneTransition;
        public void SetTriggerFlag(TriggerFlag FlagTrigger) => this.FlagTrigger = FlagTrigger;

        public virtual bool IsNested(DialogueCore dia, List<DialogueCore> history)
        {
            if (DirectTo != null && DirectTo.NewDialogue != null)
            {
                if (history.Contains(DirectTo.NewDialogue))
                    return false;

                if (DirectTo.NewDialogue == dia)
                    return true;
                else
                {
                    history.Add(this);

                    return DirectTo.NewDialogue.IsNested(dia, history);
                }
            }

            return false;
        }
    }
}
