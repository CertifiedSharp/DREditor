//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ControlsUITab : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject[] activeTabObjects;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject[] inactiveTabObjects;


        /// <summary>
        ///
        /// </summary>
        protected IControlsUIPanel activePanel;


        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        public virtual void Display(bool active, IControlsUIPanel activePanel)
        {
            this.activePanel = activePanel;

            if (FailedVerification())
                return;

            SetTabObjectStates(active);

            if (active)
                DisplayActive();
            else
                DisplayInactive();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool FailedVerification();

        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        protected virtual void SetTabObjectStates(bool active)
        {
            foreach (var tabObject in activeTabObjects)
                if (tabObject != null)
                    tabObject.SetActive(active);

            foreach (var tabObject in inactiveTabObjects)
                if (tabObject != null)
                    tabObject.SetActive(!active);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayActive();

        /// <summary>
        ///
        /// </summary>
        protected abstract void DisplayInactive();
    }
}