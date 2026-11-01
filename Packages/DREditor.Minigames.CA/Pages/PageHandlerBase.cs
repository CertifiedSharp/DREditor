// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PageHandlerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pages;


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPage"></param>
        protected virtual void UpdatePageListeners(int currentPage)
        {
            var pageListeners = transform.root.GetComponentsInChildren<IPageListener>();

            foreach (var listener in pageListeners)
                listener.OnPageUpdated(currentPage, pages.transform.childCount);
        }
    }
}