//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.ReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ReportCardComponent) +
        "\n\n")]
    public class ReportCardStudentVisuals : ReportCardComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<GameObject> silloutes = new();


        /// <inheritdoc/>
        public override void SetCardIndex(int cardIndex)
        {
            silloutes[this.cardIndex].SetActive(false);

            base.SetCardIndex(cardIndex);

            silloutes[cardIndex].SetActive(true);
        }
    }
}