//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.LocalDialogues
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface IDialogueProgression
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool AllDialogueTriggered();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool HasUnTriggeredRequiredFlags();
    }
}