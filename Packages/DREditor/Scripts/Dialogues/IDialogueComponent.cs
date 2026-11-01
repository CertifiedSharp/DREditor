//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDialogueComponent<T>
    {
        /// <summary>
        ///
        /// </summary>
        float Order { get; }

        /// <summary>
        ///
        /// </summary>
        int ExclusiveHandlerID => 1;
    }
}