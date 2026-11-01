//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TextDisplayModifier) +
        "\n\n")]
    public class HighlightTextModifier : TextDisplayModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected char encapsulateSymbol = '@';

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string highlightTextStartTag = "<gradient=\"HitText\">";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string highlightTextEndTag = "</gradient>";


        /// <inheritdoc/>
        public override string ApplyModification(string text, Line currentLine = null, TMP_Text box = null) 
            => HighlightPair(text, text.Count(x => x == encapsulateSymbol));

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="totalSymbolCount"></param>
        /// <returns></returns>
        protected virtual string HighlightPair(string text, int totalSymbolCount)
        {
            // Apply the highlighted color to the text using HTML tags
            for (int i = 0; i < totalSymbolCount; i++)
                text = TextColorInator(text, !(i % 2 == 0));

            return text;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isEndEncapsulationSymbol"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual string TextColorInator(string text, bool isEndEncapsulationSymbol)
        {
            var regex = new Regex(Regex.Escape(encapsulateSymbol.ToString()));

            string tagReplaceString = !isEndEncapsulationSymbol ? highlightTextStartTag : highlightTextEndTag;

            return regex.Replace(text, tagReplaceString, 1);
        }
    }
}