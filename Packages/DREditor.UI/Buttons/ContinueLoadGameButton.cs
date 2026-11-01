//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [RequireComponent(typeof(MoveToLoadedScene))]
	[ClassHeader("")]
    public class ContinueLoadGameButton : MainMenuButton
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent continuedGame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string applyDataEventString = "ApplyCurrentData";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool forChapterSelect = false;


        /// <inheritdoc/>
        public override void OnButtonClicked() => ContinueGame();

        /// <summary>
        ///
        /// </summary>
        public virtual void ContinueGame()
        {
            if (isLoadingFile.Value)
                return;

            var menuGroups = transform.root.GetComponentsInChildren<MenuGroupBase>();

            foreach (var group in menuGroups)
            {
                group.RemoveBackInput();

                group.StopAllCoroutines();
            }

            EventSystem.current.SetSelectedGameObject(null);

            isLoadingFile.SetValue(true);

            DialogueEventSystem.TriggerEvent(applyDataEventString, forChapterSelect);

            continuedGame.Raise();
        }
    }
}