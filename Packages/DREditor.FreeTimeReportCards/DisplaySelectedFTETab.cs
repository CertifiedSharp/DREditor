//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDisplayFTEUITab) +
        "\n\n")]
    public class DisplaySelectedFTETab : MonoBehaviour, IDisplayFTEUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<UIVisibilityComponent> fteSelectedTabs = new();


        /// <inheritdoc/>
        public virtual void DisplayFTETabData(int fteIndex)
        {
            foreach (var fte in fteSelectedTabs)
                fte.Hide();

            fteSelectedTabs[fteIndex].Show();
        }
    }
}