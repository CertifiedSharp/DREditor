//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using UnityEngine;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IResetFTEUITab) + ", " + nameof(IDisplayFTEUITab) +
        "\n\n")]
    public class ReportCardUIMainTab : MonoBehaviour, IResetFTEUITab, IDisplayFTEUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent reportCardUIPage;


        /// <inheritdoc/>
        public virtual void DisplayFTETabData(int fteIndex) => reportCardUIPage.Invoke(fteIndex <= 0);

        /// <inheritdoc/>
        public virtual void ResetComponent(bool currentCharacterIsFTECharacter) => reportCardUIPage.Show();
    }
}