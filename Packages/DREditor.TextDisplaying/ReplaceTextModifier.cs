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
    [ClassHeader(nameof(ReplaceTextModifierBase<ReplaceGroup>) +
        "\n\n")]
    public class ReplaceTextModifier : ReplaceTextModifierBase<ReplaceGroup>
    {
        /// <inheritdoc/>
        public override string ApplyModification(string text, Line currentLine = null, TMP_Text box = null)
        {
            foreach (var group in replaceGroups)
                text = ReplaceText(text, group);

            return text;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        protected virtual string ReplaceText(string text, ReplaceGroup group)
        {
            string findValue = CastUnicode(group.findValue);

            string replacedWithValue = CastUnicode(group.replacedWithValue);

            return text.Replace(findValue, replacedWithValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual string CastUnicode(string text) => text.Contains("\\") ? Regex.Unescape(text) : text;


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Add Replace Carriage")]
        public virtual void AddReplaceCarriage() => replaceGroups.Add(new ReplaceGroup("\\r", ""));
    }
}