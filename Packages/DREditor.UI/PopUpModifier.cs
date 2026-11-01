//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuModifier) +
        "\n\n")]
    public class PopUpModifier : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool skipHide = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text questionTMP = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [TextArea]
        [SerializeField] protected string questionText = "";


        [Header("")]
        [SerializeField] protected UnityEvent OnYesSelected = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnNoSelected = null;


        /// <summary>
        ///
        /// </summary>
        protected Button yesButton = null;

        /// <summary>
        ///
        /// </summary>
        protected Button noButton = null;


        protected virtual void Start() => AttemptInitializeButtons();

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptInitializeButtons()
        {
            try
            {
                InitializeText();

                InitializeButtons();

                SubscribeToButtons();
            }
            catch (IndexOutOfRangeException exception)
            {
                UnityEngine.Debug.LogWarning("ALERT: There may not be enough buttons in the pop up!");

                UnityEngine.Debug.LogException(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeText() => questionTMP.text = questionText;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeButtons()
        {
            var buttons = GetComponentsInChildren<Button>();

            yesButton = buttons[0];

            noButton = buttons[1];
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToButtons()
        {
            if (!skipHide)
                yesButton.onClick.AddListener(menu.Hide);

            noButton.onClick.AddListener(menu.Hide);

            yesButton.onClick.AddListener(OnYesSelected.Invoke);

            noButton.onClick.AddListener(OnNoSelected.Invoke);
        }


        protected virtual void OnDestroy() => UnsubscribeToButtons();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToButtons()
        {
            if (yesButton == null)
                InitializeButtons();

            yesButton.onClick.RemoveAllListeners();

            noButton.onClick.RemoveAllListeners();
        }
    }
}