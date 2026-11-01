//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    public interface IPanelManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        IEnumerator PlayPanel(Panel panel);
    }
}