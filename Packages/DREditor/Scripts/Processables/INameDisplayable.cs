//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for objects to be displayed to UI as an identification of an object.
    /// </summary>
    public interface INameDisplayable
    {
        /// <summary>
        /// The string name to be displayed to UI as an identification of an object.
        /// </summary>
        string DisplayName { get; set; }
    }
}