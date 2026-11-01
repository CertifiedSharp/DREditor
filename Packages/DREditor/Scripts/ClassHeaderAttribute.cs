//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ClassHeaderAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public string Header;


        public ClassHeaderAttribute(string header) => Header = header;
    }
}