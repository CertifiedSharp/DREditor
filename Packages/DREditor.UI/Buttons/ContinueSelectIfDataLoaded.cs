//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
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
    public class ContinueSelectIfDataLoaded : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasLoadedSaveData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent clickedNewGame = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Selectable continueSelectable = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => menu.StartEvents.AddListener(CheckIfLoad);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => menu.StartEvents.RemoveListener(CheckIfLoad);


        /// <summary>
        ///
        /// </summary>
        public virtual void CheckIfLoad()
        {
            if (!hasLoadedSaveData.Value)
                clickedNewGame.Invoke();
            else if (!isLoadingFile.Value)
                EventSystem.current.SetSelectedGameObject(continueSelectable.gameObject);
        }
    }
}