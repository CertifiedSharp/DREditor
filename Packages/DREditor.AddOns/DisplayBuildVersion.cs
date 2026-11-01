//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class DisplayBuildVersion : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI versionText;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string prefix = "v";


        protected virtual void Start()
        {
            if (versionText != null)
                versionText.text = $"{prefix}{Application.version}";
            else
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(DisplayBuildVersion)}: No TextMeshProUGUI assigned.");
        }
    }
}