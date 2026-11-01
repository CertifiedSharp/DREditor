//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class NavigationSelect
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Selectable selectable;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        public Direction direction;


        public NavigationSelect() { }

        public NavigationSelect(Selectable selectable, Direction direction)
        {
            this.selectable = selectable;

            this.direction = direction;
        }
    }
}