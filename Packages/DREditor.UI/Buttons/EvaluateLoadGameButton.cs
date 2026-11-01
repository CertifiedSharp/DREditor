//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.SaveManaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class EvaluateLoadGameButton : MenuModifier
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
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Selectable newGame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Selectable loadGame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Selectable quitGame = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int saveSlotAmount = 30;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveFilePreText = "save";


        /// <summary>
        ///
        /// </summary>
        protected MenuGroup group = null;

        
        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => menu.BeforeStart.AddListener(OnBeforeStart);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => menu.BeforeStart.RemoveListener(OnBeforeStart);


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnBeforeStart()
        {
            EvaluateLoadGameSelection();

            EvaluateLoadGameSelectability();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void EvaluateLoadGameSelection()
        {
            if (PlayerHasSaveFiles() && !clickedNewGame.Value)
                EventSystem.current.SetSelectedGameObject(loadGame.gameObject);
            else
                clickedNewGame.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayerHasSaveFiles()
        {
            for (int i = 0; i < saveSlotAmount; i++)
                if (SaveSystem.SaveFileExists(saveFilePreText + i))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void EvaluateLoadGameSelectability()
        {
            group = (MenuGroup)menu;

            if (PlayerHasSaveFiles())
                EnableLoadOption();
            else
                MakeLoadGameNotInteractable();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableLoadOption()
        {
            group.SetFirst(loadGame.gameObject);

            RewireUINavigation.SelectOnDown(newGame, loadGame);

            RewireUINavigation.SelectOnUp(quitGame, loadGame);

            loadGame.interactable = true;

            group.SetLastSelected(null);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MakeLoadGameNotInteractable()
        {
            loadGame.interactable = false;

            group.SetFirst(newGame.gameObject);

            group.SetLastSelected(null);
        }
    }
}