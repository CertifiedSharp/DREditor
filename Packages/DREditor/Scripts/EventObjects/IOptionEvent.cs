//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public interface IOptionEvent
    {
        /// <summary>
        ///
        /// </summary>
        string OptionDisplayName { get; }

        /// <summary>
        ///
        /// </summary>
        StringWithEvent DisplayDescriptionEvent { get; }

        /// <summary>
        ///
        /// </summary>
        string OptionDescription { get; }
    }
}
