//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IUpdateFTEUITab) +
        "\n\n")]
    public class DisplayFTETabsOnSelectedCharacter : MonoBehaviour, IUpdateFTEUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<UIVisibilityComponent> fteTabs = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool invert = false;


        /// <inheritdoc/>
        public virtual void UpdateUnlockedFTETabs(int characterAffinity)
        {
            for (int i = 0; i < fteTabs.Count; i++)
            {
                bool lockState = invert ? i < characterAffinity : i >= characterAffinity;

                if (i < fteTabs.Count && fteTabs[i] != null)
                    fteTabs[i].Invoke(lockState);
            }
        }
    }
}