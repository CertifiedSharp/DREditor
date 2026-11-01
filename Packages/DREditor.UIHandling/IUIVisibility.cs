//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public interface IUIVisibility
    {
        /// <summary>
        ///
        /// </summary>
        void Show();

        /// <summary>
        ///
        /// </summary>
        void Hide();

        /// <summary>
        ///
        /// </summary>
        IEnumerator WaitForChangeToComplete();
    }
}