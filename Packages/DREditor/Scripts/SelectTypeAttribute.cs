//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SelectTypeAttribute : PropertyAttribute
    {
        /// <summary>
        ///
        /// </summary>
        public string toolTipText;


        public SelectTypeAttribute(string toolTipText = "") => this.toolTipText = toolTipText;
    }
}