//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class Choice : EndEventBase
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
}