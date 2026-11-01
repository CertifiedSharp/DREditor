//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class Variable<T2> : VariableBase
    {
        [FormerlySerializedAs("BoolVariable")]
        public BoolWithEvent boolVariable;
        public BoolWithEvent BoolVariable { get => boolVariable; set => boolVariable = value; }

        public DialogueBase<T2> NextDialogueTrue;
        public int NextIndexInDialogueTrue;
        public DialogueBase<T2> NextDialogueFalse;
        public int NextIndexInDialogueFalse;
    }
    [System.Serializable]
    public class Variable : Variable<Line>
    {

    }
}
