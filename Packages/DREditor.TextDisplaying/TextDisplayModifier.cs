//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TextDisplayModifier : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentLine"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public abstract string ApplyModification(string text, Line currentLine = null, TMP_Text box = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        public virtual bool SkipLine(Line currentLine) => false;
    }
}