//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.UI.ReportCards
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ReportCardControllableComponent : ReportCardComponent
    {
        /// <summary>
        ///
        /// </summary>
        public event Action SuccessfulUINavigation;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnUINavigated() => SuccessfulUINavigation?.Invoke();


        /// <summary>
        ///
        /// </summary>
        public abstract void ActivateControls();

        /// <summary>
        ///
        /// </summary>
        public abstract void DeactivateControls();

        /// <summary>
        ///
        /// </summary>
        public abstract void ResetComponent();
    }
}