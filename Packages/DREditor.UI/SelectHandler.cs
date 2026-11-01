//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SelectHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        /// <summary>
        ///
        /// </summary>
        public abstract void Select();

        /// <summary>
        ///
        /// </summary>
        public abstract void Deselect();


        /// <summary>
        ///
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnSelect(BaseEventData eventData) => Select();

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDeselect(BaseEventData eventData) => Deselect();
    }
}