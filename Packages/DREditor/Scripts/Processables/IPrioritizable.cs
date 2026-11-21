//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for objects that have an assigned priority.
    /// </summary>
    public interface IPrioritizable
    {
        /// <summary>
        /// The priorty level for this object.
        /// </summary>
        int Priority { get; set; }
    }
}