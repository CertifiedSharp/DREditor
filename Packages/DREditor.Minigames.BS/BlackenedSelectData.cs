//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class BlackenedSelectData
    {
        /// <summary>
        ///
        /// </summary>
        public string characterName;

        /// <summary>
        ///
        /// </summary>
        public bool isAnswer = false;

        /// <summary>
        ///
        /// </summary>
        public TrialDialogue fuckUpDialogue = null;


        public BlackenedSelectData() { }

        public BlackenedSelectData(string characterName) => this.characterName = characterName;


        /// <summary>
        ///
        /// </summary>
        /// <param name="characterName"></param>
        public virtual void SetCharacterName(string characterName) => this.characterName = characterName;


        /// <summary>
        ///
        /// </summary>
        /// <param name="isAnswer"></param>
        public virtual void SetIsAnswer(bool isAnswer) => this.isAnswer = isAnswer;


        /// <summary>
        ///
        /// </summary>
        /// <param name="fuckUpDialogue"></param>
        public virtual void SetFuckUpDialogue(TrialDialogue fuckUpDialogue) => this.fuckUpDialogue = fuckUpDialogue;
    }
}