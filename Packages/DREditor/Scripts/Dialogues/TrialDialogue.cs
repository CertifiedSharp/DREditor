//Original Author: SeleniumSoul
//Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Characters;
using UnityEngine;

namespace DREditor.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Dialogues/Trial Dialogue", fileName = "New Trial Dialogue")]
    public class TrialDialogue : DialogueBased<TrialLine>, ITrialSequenceSkippable
    {
        public int skip = 0;

        public int Skip { get => skip; set => skip = value; }
    }

    public interface ITrialSequenceSkippable
    {
        int Skip { get; set; }
    }


    [System.Serializable]
    public class TrialLine : Line, ITrialLine
    {
        public TrialLine() { }

        public TrialLine(Character Speaker, int SpeakerNumber) : base(Speaker, SpeakerNumber) { }

        public TrialLine(Character Speaker, int SpeakerNumber, Expression Expression, int ExpressionNumber) :
            base(Speaker, SpeakerNumber, Expression, ExpressionNumber)
        { }

        public int camAnimIdx;

        [SerializeField] protected bool edit;//*
        public bool Edit { get => edit; set => edit = value; }
        public TCO co = null;//*

        public int CamAnimIdx { get => camAnimIdx; set => camAnimIdx = value; }

        public TCO Co { get => co; set => co = value; }
    }
}