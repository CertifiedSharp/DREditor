//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Only displays the field in the inspector if the named method evaluates to true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class ShowIfAttribute : PropertyAttribute
    {
        /// <summary>
        /// The name of the method to invoke to evaluate whether to display the targeted field to the inspector GUI.
        /// </summary>
        public string MethodName { get; private set; }


        /// <summary>
        /// Only displays the field in the inspector if the named method evaluates to true.
        /// </summary>
        /// <param name="methodName">
        /// The name of the method to invoke to evaluate whether to display the targeted field to the inspector GUI.
        /// </param>
        public ShowIfAttribute(string methodName) => MethodName = methodName;
    }
}