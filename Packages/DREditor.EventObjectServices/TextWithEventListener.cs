//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class TextWithEventListener : StringWithEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string prefix = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string suffix = " ";


        /// <summary>
        ///
        /// </summary>
        protected TMP_Text TMP_Text => GetComponent<TMP_Text>();


        /// <inheritdoc/>
        protected override void OnValueChanged(string updatedValue)
        {
            base.OnValueChanged(updatedValue);

            UpdateTextWithString(updatedValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="updatedValue"></param>
        protected virtual void UpdateTextWithString(string updatedValue) => TMP_Text.text = prefix + updatedValue + suffix;
    }
}