//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    public interface IReactionEntry
    {
        /// <summary>
        ///
        /// </summary>
        Character Character { get; set; }

        /// <summary>
        ///
        /// </summary>
        int ReactionIndex { get; set; }
    }
}