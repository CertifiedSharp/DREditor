//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for objects or systems that contain a description, likely to be displayed to UI.
    /// </summary>
    public interface IDescriptable
    {
        /// <summary>
        /// Description being handled by this script, likely to be displayed to UI.
        /// </summary>
        string Description { get; set; }
    }
}