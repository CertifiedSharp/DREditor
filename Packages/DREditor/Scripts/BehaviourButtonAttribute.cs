//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BehaviourButtonAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Label { get; }


        public BehaviourButtonAttribute(string label = null) => Label = label;
    }
}