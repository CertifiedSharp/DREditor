//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SymbolTextModifier) +
        "\n\n")]
    public class SymbolColorTextModifier : SymbolTextModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] Color32 newTextColor = new(77, 193, 250, 255);


        /// <inheritdoc/>
        protected override void OnSymbolFound(string text, Line currentLine = null, TMP_Text box = null) => box.color = newTextColor;
    }
}