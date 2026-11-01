//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface ITextDisplayerHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        TMP_Text GetTextBox();

        /// <summary>
        ///
        /// </summary>
        void ClearText();
    }
}