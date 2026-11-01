//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public class UIVisibilityOnDialogueEvent : UIVisibilityHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected string showMainUIEventString = "ShowMainUI"; // DialogueUI: ShowBox MoveDialogue

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideMainUIEventString = "HideMainUI"; // DialogueUI: HideBox MoveBackDialogue


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => SubscribeToDialogueEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToDialogueEvents()
        {
            DialogueEventSystem.StartListening(showMainUIEventString, ShowUIEvent);

            DialogueEventSystem.StartListening(hideMainUIEventString, HideUIEvent);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnsubscribeToDialogueEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToDialogueEvents()
        {
            DialogueEventSystem.StopListening(showMainUIEventString, ShowUIEvent);

            DialogueEventSystem.StopListening(hideMainUIEventString, HideUIEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void ShowUIEvent(object o = null) => visibleUI.Show();

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void HideUIEvent(object o = null) => visibleUI.Hide();
    }
}