//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    public interface IControlsUIPanel
    {
        /// <summary>
        ///
        /// </summary>
        string Title { get; }

        /// <summary>
        ///
        /// </summary>
        IControlRowData[] Rows { get; }

        /// <summary>
        ///
        /// </summary>
        string SystemDescription { get; }
    }
}