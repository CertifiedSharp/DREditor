//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PartlyHitModifier : HittableModifierBase, IMistakeModifier, ITextByArray
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected string[] partlyHitableWordArray;

        /// <summary>
        ///
        /// </summary>
        public string[] WordArray { get => partlyHitableWordArray; set => partlyHitableWordArray = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int partlyHitableWordStart;

        /// <summary>
        ///
        /// </summary>
        public int WordStart { get => partlyHitableWordStart; set => partlyHitableWordStart = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int partlyHitableWordCount;

        /// <summary>
        ///
        /// </summary>
        public int WordCount { get => partlyHitableWordCount; set => partlyHitableWordCount = value; }

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected string partlyHitablePhraseText;

        /// <summary>
        ///
        /// </summary>
        public string Text { get => partlyHitablePhraseText; set => partlyHitablePhraseText = value; }


        /// <inheritdoc/>
        public override string DisplayName 
            => string.IsNullOrWhiteSpace(Text) ? 
            base.DisplayName : 
            base.DisplayName + ": " + GetDisplayText();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDisplayText() => "==" + Text + "==";


        /// <inheritdoc/>
        public virtual bool UsedForMistake() => WordCount > 0;

        /// <inheritdoc/>
        public virtual void SetText(string text) => Text = text;

        /// <inheritdoc/>
        public virtual void SetStart(int start) => WordStart = start;

        /// <inheritdoc/>
        public virtual void SetCount(int count) => WordCount = count;
    }
}