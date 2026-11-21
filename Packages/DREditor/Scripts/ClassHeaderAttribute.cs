//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor
{
    /// <summary>
    /// When applied to a class that does not use a custom editor, applies a header at the top of the script's inspector GUI.
    /// </summary>
    /// <remarks>
    /// If this attribute is on a class that has a custom editor it should be removed.
    /// <br/>
    /// This attribute was made to get around having to require a field to put a header from classes that inherited other classes
    /// but didn't have any additional fields.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ClassHeaderAttribute : Attribute
    {
        /// <summary>
        /// The header text to display at the very top of the monobehaviour in the inspector.
        /// </summary>
        public string Header;


        /// <summary>
        /// When applied to a class that does not use a custom editor, applies a header at the top of the script's inspector GUI.
        /// </summary>
        /// <param name="header">The header text to display at the very top of the monobehaviour in the inspector.</param>
        public ClassHeaderAttribute(string header) => Header = header;
    }
}