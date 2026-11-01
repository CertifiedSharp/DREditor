//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDisplayFTEUITab) +
        "\n\n")]
    public class DisplayFTEData : MonoBehaviour, IDisplayFTEUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool hideOnStart = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent fteText = null;


        protected virtual void Start() => StartCoroutine(LateStart());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator LateStart()
        {
            yield return null;

            fteText.Invoke(!hideOnStart);
        }

        /// <inheritdoc/>
        public virtual void DisplayFTETabData(int fteIndex) => fteText.Invoke(fteIndex > 0);
    }
}