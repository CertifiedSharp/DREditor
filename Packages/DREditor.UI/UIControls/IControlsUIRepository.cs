//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    public interface IControlsUIRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IControlsUIPanel GetPanel(string id);
    }
}