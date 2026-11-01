//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class Phrase : PhraseBase, IDebateScanner, ITextByArray
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [SerializeField] protected int phraseStart;

        public int WordStart { get => phraseStart; set => phraseStart = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int wordCount;

        public int WordCount { get => wordCount; set => wordCount = value; }

        /// <summary>
        ///
        /// </summary>
        public DialogueCore fuckUpDialogue;

        /// <summary>
        ///
        /// </summary>
        /// <param name="fuckUpDialogue"></param>
        public virtual void SetFuckUpDialogue(DialogueCore fuckUpDialogue) => this.fuckUpDialogue = fuckUpDialogue;


        /// <inheritdoc/>
        public virtual bool Scan(out string log)
        {
            log = "";

            if (fuckUpDialogue != null)
                return false;

            foreach(var mod in modifiers)
                if (mod is IMistakeModifier mistakeModifier && mistakeModifier.UsedForMistake())
                {
                    log = "Missing FU Dialogue.";

                    return true;
                }

            return false;
        }

        /// <inheritdoc/>
        public virtual void SetText(string text) => Text = text;

        /// <inheritdoc/>
        public virtual void SetStart(int start) => WordStart = start;

        /// <inheritdoc/>
        public virtual void SetCount(int count) => WordCount = count;
    }
}