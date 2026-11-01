//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class NavigationGroup
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public OptionUIGroupBase group;

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        public Direction direction;


        public NavigationGroup() { }

        public NavigationGroup(OptionUIGroupBase group, Direction direction)
        {
            this.group = group;

            this.direction = direction;
        }
    }
}