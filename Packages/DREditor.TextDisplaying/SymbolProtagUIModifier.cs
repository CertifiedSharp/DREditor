//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SymbolTextModifier) + ", " + nameof(ILineRoutine) +
        "\n\n")]
    public class SymbolProtagUIModifier : SymbolTextModifier, ILineRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent toggleProtagUI = null;


        /// <inheritdoc/>
        protected override void OnSymbolFound(string text, Line currentLine = null, TMP_Text box = null) => toggleProtagUI.Raise();


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine) => currentLine.Text.Contains(startingSymbol);

        /// <inheritdoc/>
        public override bool SkipLine(Line currentLine)
        {
            bool onlySymbol = string.IsNullOrEmpty(currentLine.Text.Replace(startingSymbol.ToString(), ""));

            if (onlySymbol)
                OnSymbolFound(currentLine.Text, currentLine, null);

            return onlySymbol || base.SkipLine(currentLine);
        }
    }
}