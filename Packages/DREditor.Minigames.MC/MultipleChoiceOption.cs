//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;

namespace DREditor.Minigames.MC
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class MultipleChoiceOption
    {
        /// <summary>
        ///
        /// </summary>
        public string text = "";

        /// <summary>
        ///
        /// </summary>
        public bool isAnswer = false;

        /// <summary>
        ///
        /// </summary>
        public TrialDialogue wrongDialogue;


        /// <summary>
        ///
        /// </summary>
        public virtual void SetText(string text) => this.text = text;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetIsAnswer(bool isAnswer) => this.isAnswer = isAnswer;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetWrongDialogue(TrialDialogue wrongDialogue)
            => this.wrongDialogue = wrongDialogue;
    }
}