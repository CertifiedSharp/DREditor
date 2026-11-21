//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for objects or systems that can be identified through an integer.
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// The integer ID for this script.
        /// </summary>
        int ID { get; }
    }
}