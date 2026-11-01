//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IToTitleScreen) + 
        "\n\n")]
    public class ProtagUIWindow : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent protagUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> dialogueEventsOnShow = new()
        {
            "EnableProtagCamera", "CaptureProtag", "MoveDialogue"
        };

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> dialogueEventsOnHide = new()
        {
            "MoveBackDialogue", "DisableProtagCamera"
        };

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent protagUIVisible = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => protagUIVisible.AddListener(OnProtagUIVisibilityChanged);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => protagUIVisible.RemoveListener(OnProtagUIVisibilityChanged);

        /// <summary>
        ///
        /// </summary>
        /// <param name="visibleValue"></param>
        protected void OnProtagUIVisibilityChanged(bool visibleValue)
        {
            if (visibleValue)
                ShowProtag();
            else
                HideProtag();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowProtag()
        {
            DialogueEventSystem.TriggerEvents(dialogueEventsOnShow);

            if (protagUI != null && protagUI is IUIVisibility visibleUI)
                visibleUI.Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideProtag()
        {
            if (protagUI != null && protagUI is IUIVisibility visibleUI)
                visibleUI.Hide();

            DialogueEventSystem.TriggerEvents(dialogueEventsOnHide);
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            if (protagUIVisible.Value)
                protagUIVisible.SetValue(false);
        }
    }
}