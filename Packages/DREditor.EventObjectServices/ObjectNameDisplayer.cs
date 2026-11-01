//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using TMPro;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(StringWithEventListener) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ObjectNameDisplayer : StringWithEventListener, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent displayNameVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI displayText = null;


        /// <inheritdoc/>
        protected override void OnValueChanged(string updatedValue)
        {
            base.OnValueChanged(updatedValue);

            if (updatedValue != "")
                displayText.text = updatedValue;

            displayNameVisible.SetValue(updatedValue != "");
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => displayNameVisible.SetValue(false);
    }
}