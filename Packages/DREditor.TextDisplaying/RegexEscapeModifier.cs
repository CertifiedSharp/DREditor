//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
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
    public class RegexEscapeModifier : TextDisplayModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string regexString = @"(?: *)\\(?: *)u(?: *)(.)(?: *)(?: *)(.)(.)(.)";


        /// <inheritdoc/>
        public override string ApplyModification(string text, Line currentLine = null, TMP_Text box = null)
            => ApplyRegex(text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual string ApplyRegex(string text)
        {
            Regex reg = new(regexString);

            MatchCollection m = reg.Matches(text);

            foreach (Match match in m)
                text = reg.Replace(text, Regex.Unescape(match.Value));

            return text;
        }
    }
}