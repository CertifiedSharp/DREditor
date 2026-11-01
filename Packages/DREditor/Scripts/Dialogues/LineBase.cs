//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public abstract class LineBase
    {
        public Character Speaker;

        public int SpeakerNumber = -1;

        public Expression Expression;

        public int ExpressionNumber;

        protected LineBase() { }

        protected LineBase(Character Speaker, int SpeakerNumber)
        {
            this.Speaker = Speaker;

            this.SpeakerNumber = SpeakerNumber;
        }

        protected LineBase(Character Speaker, int SpeakerNumber, Expression Expression, int ExpressionNumber) :
            this(Speaker, SpeakerNumber)
        {
            this.Expression = Expression;

            this.ExpressionNumber = ExpressionNumber;
        }

        public void SetSpeaker(Character Speaker) => this.Speaker = Speaker;

        public void SetSpeakerNum(int SpeakerNumber) => this.SpeakerNumber = SpeakerNumber;

        public void SetExpression(Expression Expression) => this.Expression = Expression;

        public void SetExpressionNum(int ExpressionNumber) => this.ExpressionNumber = ExpressionNumber;
    }
}