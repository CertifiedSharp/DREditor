//Author: Happy30
//Completely Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class Choice : EndEventBase, IChoice
    {
        [FormerlySerializedAs("ChoiceText")]
        public string choiceText;
        public string ChoiceText { get => choiceText; set => choiceText = value; }

        [FormerlySerializedAs("NextDialogue")]
        public Dialogue nextDialogue;
        public Dialogue NextDialogue { get => nextDialogue; set => nextDialogue = value; }

        [FormerlySerializedAs("NextIndexInDialogue")]
        public int nextIndexInDialogue;
        public int NextIndexInDialogue { get => nextIndexInDialogue; set => nextIndexInDialogue = value; }

        [FormerlySerializedAs("CustomCall")]
        public string customCall;
        public string CustomCall { get => customCall; set => customCall = value; }

        [FormerlySerializedAs("CallBreak")]
        public bool callBreak = false;
        public bool CallBreak { get => callBreak; set => callBreak = value; }
    }

    [System.Serializable]
    public class ChoiceEvent : EndEventBase
    {
        [SelectType]
        [SerializeReference]
        protected List<IChoice> choices = new();


        public virtual List<IChoice> Choices => choices;
    }

    public interface IChoice
    {
        string ChoiceText { get; set; }
    }
}