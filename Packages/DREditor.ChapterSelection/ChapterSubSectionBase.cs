//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ChapterSubSectionBase : ChapterSectionBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase startGameGroup = null;


        /// <summary>
        ///
        /// </summary>
        protected readonly List<UnityAction> buttonActions = new();


        protected virtual void OnEnable() => SubscribeButtonSelection();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeButtonSelection()
        {
            foreach (var button in sectionButtons)
            {
                void ButtonAction() => SetSelectedSection(button);

                button.onClick.AddListener(ButtonAction);

                button.onClick.AddListener(ChangeGroup);

                buttonActions.Add(ButtonAction);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeGroup()
        {
            if (startGameGroup != null && TryGetComponent(out MenuGroupBase menuGroup))
                menuGroup.ChangeGroup(startGameGroup);
            else if (startGameGroup == null)
                UnityEngine.Debug.LogError($"ALERT: The field: {nameof(startGameGroup)} has not been assigned!");
            else
                UnityEngine.Debug.LogError($"ALERT: There is no Menu Group on {gameObject.name} when there must be!");
        }


        protected virtual void OnDisable() => UnsubscribeButtonSelection();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeButtonSelection()
        {
            for (int i = 0; i < sectionButtons.Count; i++)
            {
                if (i < buttonActions.Count)
                    sectionButtons[i].onClick.RemoveListener(buttonActions[i]);

                sectionButtons[i].onClick.RemoveListener(ChangeGroup);
            }

            buttonActions.Clear();
        }
    }
}