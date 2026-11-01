//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using UnityEngine;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IResetFTEUITab) +
        "\n\n")]
    public class DisplayFTEUITabsOnReset : MonoBehaviour, IResetFTEUITab
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent fteUIPage;


        /// <inheritdoc/>
        public virtual void ResetComponent(bool currentCharacterIsFTECharacter) => fteUIPage.Invoke(currentCharacterIsFTECharacter);
    }
}