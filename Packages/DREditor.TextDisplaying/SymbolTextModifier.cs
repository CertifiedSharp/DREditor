//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SymbolTextModifier : TextDisplayModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected char startingSymbol = '$';


        /// <inheritdoc/>
        public override string ApplyModification(string text, Line currentLine = null, TMP_Text box = null)
        {
            if (TextStartsWithSymbol(text))
            {
                OnSymbolFound(text, currentLine, box);

                text = text.Remove(text.IndexOf(startingSymbol), 1);
            }

            return text;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual bool TextStartsWithSymbol(string text) => text.Split(' ')[0].Contains(startingSymbol);

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentLine"></param>
        /// <param name="box"></param>
        protected abstract void OnSymbolFound(string text, Line currentLine = null, TMP_Text box = null);
    }
}