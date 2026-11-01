//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyButtonAttribute : PropertyAttribute
    {
        /// <summary>
        ///
        /// </summary>
        public string methodName;

        /// <summary>
        ///
        /// </summary>
        public string buttonText;


        public PropertyButtonAttribute(string methodName, string buttonText = "Invoke")
        {
            this.methodName = methodName;

            this.buttonText = buttonText;
        }
    }
}