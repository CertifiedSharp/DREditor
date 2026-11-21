//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class ShowIfAttribute : PropertyAttribute
    {
        /// <summary>
        ///
        /// </summary>
        public string MethodName { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        public ShowIfAttribute(string methodName) => MethodName = methodName;
    }
}