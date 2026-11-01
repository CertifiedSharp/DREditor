//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public class CGLineTextDisplayer : DialogueLineTextDisplayer
    {
        /// <inheritdoc/>
        protected override bool SkipDisplayingText() => !inCGMode.Value && !inVideoMode.Value;
    }
}