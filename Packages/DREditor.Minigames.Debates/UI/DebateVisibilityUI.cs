//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IDebateUIVisibility) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class DebateVisibilityUI : UIVisibilityComponent, IDebateUIVisibility
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent = null;


        /// <inheritdoc/>
        protected override void HideUI() => visibilityComponent.Hide();

        /// <inheritdoc/>
        protected override void ShowUI() => visibilityComponent.Show();

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete() => visibilityComponent.WaitForChangeToComplete();
    }
}