//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System;

namespace DREditor
{
    /// <summary>
    /// Base class for a custom class editor for POCO of type: <typeparamref name="T"/>. <br/>
    /// Contains helper function(s) for displaying fields.
    /// </summary>
    /// <typeparam name="T">The target class type being edited.</typeparam>
    public abstract class ClassEditorTypeBase<T> : ClassEditorBase
    {
        /// <summary>
        /// The target <typeparamref name="T"/> being edited.
        /// </summary>
        protected T target;


        /// <inheritdoc/>
        public override void OnGUI(object target, params object[] objects)
        {
            this.target = (T)target;

            base.OnGUI(target, objects);
        }


        /// <summary>
        /// Helper function that displays and sets a field by it's property name.
        /// </summary>
        /// <typeparam name="T2">Type of field to be displayed.</typeparam>
        /// <param name="label">The GUIContent label next to the field.</param>
        /// <param name="propertyName">Name of the property to set.</param>
        /// <param name="display">Function to display the type of field.</param>
        protected virtual void DisplayField<T2>(string label, string propertyName, Func<string, T2, T2> display)
        {
            var property = target.GetType().GetProperty(propertyName);

            T2 current = (T2)property.GetValue(target);

            T2 value;

            value = display(label, current);

            if (Equals(value, current))
                return;

            property.SetValue(target, value);
        }
    }
}
#endif