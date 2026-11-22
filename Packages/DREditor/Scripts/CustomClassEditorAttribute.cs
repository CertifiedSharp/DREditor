//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System;

namespace DREditor
{
    /// <summary>
    /// Uses the class this attribute is attached to in order to edit the POCO of the type specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CustomClassEditorAttribute : Attribute
    {
        /// <summary>
        /// The target type for the custom editor.
        /// </summary>
        public Type TargetType { get; private set; }


        /// <summary>
        /// Uses the class this attribute is attached to in order to edit <paramref name="targetType"/>
        /// </summary>
        /// <param name="targetType">The target type for the custom editor.</param>
        public CustomClassEditorAttribute(Type targetType) => TargetType = targetType;
    }
}
#endif