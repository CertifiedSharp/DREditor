//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class QuitButton : MainMenuButton
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public override void OnButtonClicked() => Application.Quit();
    }
}