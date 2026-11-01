//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(UIBehaviour) + ", " + nameof(ISelectHandler) + ", " + nameof(IDeselectHandler) +
        "\n\n")]
    public class ChangerUIList : UIBehaviour, ISelectHandler, IDeselectHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SelectHandler> changers = new();


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

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Select()
        {
            foreach(SelectHandler changer in changers)
                changer.Select();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Deselect()
        {
            foreach (SelectHandler changer in changers)
                changer.Deselect();
        }
    }
}