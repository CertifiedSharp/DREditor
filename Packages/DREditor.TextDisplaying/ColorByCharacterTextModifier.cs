//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TextDisplayModifier) +
        "\n\n")]
    public class ColorByCharacterTextModifier : TextDisplayModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string characterFirstName = "Tutorial";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color characterTextColor = new(0.27776733f, 0.735849023f, 0.136525407f, 255);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color32 normalColor = new(255, 255, 255, 255);


        /// <summary>
        ///
        /// </summary>
        public override string ApplyModification(string text, Line currentLine = null, TMP_Text box = null)
        {
            bool useTutorialColor = currentLine.Speaker != null && currentLine.Speaker.FirstName.Equals(characterFirstName);

            box.color = useTutorialColor ? characterTextColor : normalColor;

            return text;
        }
    }
}