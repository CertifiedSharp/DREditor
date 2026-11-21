//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Attribute that displays a drop down of all types that inherits the field type so the user can choose which type to use.
    /// </summary>
    /// <remarks>
    /// It should be noted that the reason this attribute is used given that the TypeList class exists
    /// is because TypeList is focused on the same functionality as this attribute except applicable to custom editors only. <br/>
    /// Where as this attribute can be applied for scripts without custom editors only.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SelectTypeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Text to display for the drop down's tool tip.
        /// </summary>
        public string toolTipText;


        /// <summary>
        /// Attribute that displays a drop down of all types that inherits the field type so the user can choose which type to use.
        /// </summary>
        /// <param name="toolTipText">Text to display for the drop down's tool tip.</param>
        /// <remarks>
        /// It should be noted that the reason this attribute is used given that the TypeList class exists
        /// is because TypeList is focused on the same functionality as this attribute except applicable to custom editors only. <br/>
        /// Where as this attribute can be applied for scripts without custom editors only.
        /// </remarks>
        public SelectTypeAttribute(string toolTipText = "") => this.toolTipText = toolTipText;
    }
}