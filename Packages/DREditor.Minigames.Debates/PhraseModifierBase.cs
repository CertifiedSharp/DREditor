//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Text.RegularExpressions;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class PhraseModifierBase : IPhraseModifier
    {
        /// <inheritdoc/>
        public virtual string DisplayName
        {
            get => Regex.Replace(GetType().Name, "([A-Z])", " $1").Trim();

            set => Regex.Replace(GetType().Name, "([A-Z])", " $1").Trim();
        }
    }
}