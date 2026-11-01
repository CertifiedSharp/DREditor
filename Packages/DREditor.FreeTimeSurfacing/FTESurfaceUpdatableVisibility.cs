//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.UIHandling;
using UnityEngine;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FTESurfaceComponent) + ", " + nameof(ISurfacable) +
        "\n\n")]
    public class FTESurfaceUpdatableVisibility : FTESurfaceComponent, ISurfacable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiDisplayComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool swap = false;


        /// <inheritdoc/>
        public virtual void StartUp()
        {
            if (!swap)
                Show();
            else
                Hide();
        }

        /// <inheritdoc/>
        public virtual void Close()
        {
            if (!swap)
                Hide();
            else
                Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Show()
        {
            if (uiDisplayComponent != null)
                uiDisplayComponent.Show();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Hide()
        {
            if (uiDisplayComponent != null)
                uiDisplayComponent.Hide();
        }
    }
}