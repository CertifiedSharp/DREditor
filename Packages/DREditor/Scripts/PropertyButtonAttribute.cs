//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Attribute that can be applied to any field, to place an inspector button above it to call a specified function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyButtonAttribute : PropertyAttribute
    {
        /// <summary>
        /// The string name of the method to call, when the button is pressed from the inspector. (Use nameof())!!
        /// </summary>
        public string methodName;

        /// <summary>
        /// The text to put on the button that calls the function from the inspector.
        /// </summary>
        public string buttonText;


        /// <summary>
        /// Attribute that can be applied to any field, to place an inspector button above it to call a specified function.
        /// </summary>
        /// <param name="methodName">The string name of the method to call, when the button is pressed from the inspector. (Use nameof())!!</param>
        /// <param name="buttonText">The text to put on the button that calls the function from the inspector.</param>
        public PropertyButtonAttribute(string methodName, string buttonText = "Invoke")
        {
            this.methodName = methodName;

            this.buttonText = buttonText;
        }
    }
}