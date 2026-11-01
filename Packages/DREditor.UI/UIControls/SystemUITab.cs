//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlsUITab) +
        "\n\n")]
    public class SystemUITab : ControlsUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI systemDescription = null;


        /// <inheritdoc/>
        protected override bool FailedVerification() => string.IsNullOrEmpty(activePanel.SystemDescription);

        /// <inheritdoc/>
        protected override void DisplayActive() => systemDescription.text = activePanel.SystemDescription;

        /// <inheritdoc/>
        protected override void DisplayInactive() { }
    }
}