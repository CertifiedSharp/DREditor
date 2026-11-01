//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    public abstract class OptionUIGroupBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<Selectable> options = new();


        /// <summary>
        ///
        /// </summary>
        protected ISelectHandler[] selectHandlers;

        /// <summary>
        ///
        /// </summary>
        protected IDeselectHandler[] deselectHandlers;


        protected virtual void Awake()
        {
            selectHandlers = GetComponents<ISelectHandler>();

            deselectHandlers = GetComponents<IDeselectHandler>();
        }
    }
}