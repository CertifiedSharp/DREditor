//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor
{
    /// <summary>
    /// Attribute that can be applied to any method, to show an inspector button that will call the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BehaviourButtonAttribute : Attribute
    {
        /// <summary>
        /// The text to put on the button that calls the function from the inspector.
        /// </summary>
        public string Label { get; }


        /// <summary>
        /// Attribute that can be applied to any method, to show an inspector button that will call the method.
        /// </summary>
        /// <param name="label">The text to put on the button that calls the function from the inspector.</param>
        public BehaviourButtonAttribute(string label = null) => Label = label;
    }
}