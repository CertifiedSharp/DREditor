//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Investigations;

namespace DREditor.Minigames.TBS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class TBSelection
    {
        /// <summary>
        ///
        /// </summary>
        public TruthBullet bulletOption;

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
        /// <param name="bulletOption"></param>
        public void SetBullet(TruthBullet bulletOption) => this.bulletOption = bulletOption;

        /// <summary>
        ///
        /// </summary>
        /// <param name="isAnswer"></param>
        public void SetAnswer(bool isAnswer) => this.isAnswer = isAnswer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="wrongDialogue"></param>
        public void SetWrongDialogue(TrialDialogue wrongDialogue) => this.wrongDialogue = wrongDialogue;
    }
}