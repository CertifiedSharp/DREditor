//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    public class DialogueBase<T> : DialogueCore, IEndEventCollection
    {

        [FormerlySerializedAs("Lines")]
        public List<T> lines = new();
        public List<T> Lines { get => lines; }

        [SelectType]
        [SerializeReference]
        protected List<IEndEvent> endEvents = new();

        public virtual List<IEndEvent> EndEvents { get => endEvents; }

        public virtual T2 GetEndEvent<T2>()
        {
            foreach (var endEvent in EndEvents)
                if (endEvent is T2 result)
                    return result;

            return default;
        }

        public SceneTransition SceneTransition = new();
        
        public TriggerFlag FlagTrigger = new(); //*

        public override int LineCount { get => Lines.Count; }

        public override void InvokeDialogue(ITypeCallback callback) { }
        

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

        public Variable Variable = new();
        public DirectTo DirectTo;

        public void SetVariable(Variable Variable) => this.Variable = Variable;
        public void SetDirectTo(DirectTo DirectTo) => this.DirectTo = DirectTo;
        public void SetSceneTransition(SceneTransition SceneTransition) => this.SceneTransition = SceneTransition;
        public void SetTriggerFlag(TriggerFlag FlagTrigger) => this.FlagTrigger = FlagTrigger;

        public override bool IsNested(DialogueCore dia, List<DialogueCore> history)
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



#if UNITY_EDITOR
        [SerializeField] protected bool converted = false;

        protected void SetConverted() => converted = true;

        public override void ConvertEndEvents()
        {
            if (converted)
                return;

            ConvertEndEventInstances();

            ClearOldEndEvents();

            SetConverted();

            UnityEditor.EditorUtility.SetDirty(this);
        }


        protected virtual void ConvertEndEventInstances()
        {
            AddExistingEvent(SceneTransition);

            AddExistingEvent(FlagTrigger);

            AddExistingEvent(DirectTo);

            AddExistingEvent(Variable);
        }

        protected virtual void AddExistingEvent(EndEventBase endEvent)
        {
            if (endEvent != null && endEvent.Enabled)
                EndEvents.Add(endEvent);
        }

        protected virtual void ClearOldEndEvents()
        {
            SceneTransition = null;

            FlagTrigger = null;

            DirectTo = null;

            Variable = null;
        }
#endif
    }


    public interface IEndEventCollection
    {
        List<IEndEvent> EndEvents { get; }

        T GetEndEvent<T>();
    }

    public interface IEndEvent
    {
        bool IsValid();
    }
    [System.Serializable]
    public class DialogueEventEndEvent : EndEventBase, IDialogueEventCollection
    {
        [SerializeReference]
        protected List<IDialogueEvent> dialogueEvents = new();


        public virtual List<IDialogueEvent> DialogueEvents => dialogueEvents;
    }
}
