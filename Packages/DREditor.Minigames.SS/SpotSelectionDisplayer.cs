//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SpotSelectionDisplayer : SpotSelectionComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent displayer = null;


        /// <inheritdoc/>
        public override void Show() => displayer.Show();

        /// <inheritdoc/>
        public override void Hide() => displayer.Hide();

        /// <inheritdoc/>
        public override IEnumerator WaitForChangeToComplete() => displayer.WaitForChangeToComplete();
    }
}