//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.EventObjects;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomPropertyDrawer(typeof(BaseValueWithEvent), true)]
    public class ValueWithEventDrawer : PropertyDrawer
    {
        /// <summary>
        ///
        /// </summary>
        const string ValuePropertyName = "_value";

        /// <summary>
        ///
        /// </summary>
        const string CurrentValueLabel = "Current Value";

        /// <summary>
        ///
        /// </summary>
        const float MainFieldOffset = 2;

        /// <summary>
        ///
        /// </summary>
        const float HeightOffset = 4;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float mainFieldHeight = EditorGUI.GetPropertyHeight(property, label, true);

            Rect mainFieldRect = new(position.x, position.y, position.width, mainFieldHeight);

            EditorGUI.PropertyField(mainFieldRect, property, label, true);

            if (property.objectReferenceValue != null)
            {
                SerializedObject obj = new(property.objectReferenceValue);

                SerializedProperty valueProp = obj.FindProperty(ValuePropertyName);

                if (valueProp != null)
                {
                    Rect valueRect = new(position.x, position.y + mainFieldHeight + MainFieldOffset,
                        position.width, EditorGUIUtility.singleLineHeight);

                    EditorGUI.BeginDisabledGroup(true);

                    EditorGUI.PropertyField(valueRect, valueProp, new GUIContent(CurrentValueLabel));

                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label, true);

            if (property.objectReferenceValue != null)
                return height + EditorGUIUtility.singleLineHeight + HeightOffset;

            return height;
        }
    }
}