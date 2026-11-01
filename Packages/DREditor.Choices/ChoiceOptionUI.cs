//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using TMPro;
using UnityEngine;

namespace DREditor.Choices
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ChoiceOptionUI : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI choiceText = null;


        /// <summary>
        ///
        /// </summary>
        protected event Action OnChoiceSelected;


        /// <summary>
        ///
        /// </summary>
        public virtual void InvokeChoice() => OnChoiceSelected?.Invoke();

        /// <summary>
        ///
        /// </summary>
        public virtual void SubscribeChoice(Action action) => OnChoiceSelected += action;

        /// <summary>
        ///
        /// </summary>
        public virtual void UnsubscribeChoice(Action action) => OnChoiceSelected -= action;

        /// <summary>
        ///
        /// </summary>
        public virtual void SetText(string text) => choiceText.text = text;

        /// <summary>
        ///
        /// </summary>
        public virtual void Clear() => choiceText.text = "";
    }
}