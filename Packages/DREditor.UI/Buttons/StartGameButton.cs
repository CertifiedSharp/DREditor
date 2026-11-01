//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(MoveToScene))]
	[ClassHeader("")]
    public class StartGameButton : MainMenuButton
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent startedNewWithLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasLoadedSaveData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent newGameSelected = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent startedNewGame = null;


        /// <inheritdoc/>
        public override void OnButtonClicked() => StartNewGame();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void StartNewGame()
        {
            var menuGroups = transform.root.GetComponentsInChildren<MenuGroupBase>();

            foreach (var group in menuGroups)
                group.RemoveBackInput();

            EventSystem.current.SetSelectedGameObject(null);

            if (hasLoadedSaveData.Value)
                startedNewWithLoaded.SetValue(true);
            else
                newGameSelected.Raise();
            // The above originally did not have the else statement.
            // However, by adding the else, we solve a scenario where saved data is deleted in scenario:
            // Load Game File -> Pause Return To Menu -> New Game -> Pause Return To Menu -> Continue
            // If issues arrive as a result of the above else statement addition, then a solution that also 
            // solves the above mentioned scenario must be taken into account otherwise the scenario issue will be present.

            startedNewGame.Raise();
        }
    }
}