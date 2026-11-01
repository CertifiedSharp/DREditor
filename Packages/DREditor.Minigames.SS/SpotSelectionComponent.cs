//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SpotSelectionComponent : MonoBehaviour, IUIVisibility
    {
        /// <summary>
        ///
        /// </summary>
        public abstract void Initialize(SpotSelectionBuilder builder);

        /// <inheritdoc/>
        public abstract void Show();

        /// <inheritdoc/>
        public abstract void Hide();

        /// <inheritdoc/>
        public abstract IEnumerator WaitForChangeToComplete();
    }
}