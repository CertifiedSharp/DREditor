//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class NewGameButton : MainMenuButton
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent clickedNewGame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent newGameSelected = null;


        /// <inheritdoc/>
        public override void OnButtonClicked() => SelectedNewGame();

        /// <summary>
        ///
        /// </summary>
        public virtual void SelectedNewGame()
        {
            clickedNewGame.SetValue(true);

            newGameSelected.Raise();
        }
    }
}