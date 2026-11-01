//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        /// <summary>
        ///
        /// </summary>
        private bool showField = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            object target = property.propertyType == SerializedPropertyType.ManagedReference?
                property.managedReferenceValue : GetParent(property);

            if (target != null)
                EvaluateDisplayingField(target);

            if (showField)
                EditorGUI.PropertyField(position, property, label, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        protected virtual void EvaluateDisplayingField(object target)
        {
            Type targetType = target.GetType();

            ShowIfAttribute showIfAttr = (ShowIfAttribute)attribute;

            MethodInfo method = targetType.GetMethod(showIfAttr.MethodName,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null && method.ReturnType == typeof(bool))
                showField = (bool)method.Invoke(method.IsStatic ? null : target, null);
            else if (method != null)
                UnityEngine.Debug.LogWarning($"ALERT: Method '{showIfAttr.MethodName}' returning bool not found on {targetType.Name}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetParent(SerializedProperty property)
        {
            string path = property.propertyPath.Replace(".Array.data[", "[");

            string[] elements = path.Split('.');

            object obj = property.serializedObject.targetObject;

            for (int i = 0; i < elements.Length - 1; i++)
                obj = GetValue(obj, elements[i]);

            return obj;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private object GetValue(object source, string name, int index = -1)
        {
            if (source == null) return null;

            var type = source.GetType();

            var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            while (field == null && type.BaseType != null)
            {
                type = type.BaseType;

                field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (field == null)
                return null;

            var value = field.GetValue(source);

            if (index != -1 && value is System.Collections.IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();

                int i = 0;

                while (enumerator.MoveNext())
                {
                    if (i == index)
                        return enumerator.Current;

                    i++;
                }

                return null;
            }

            return value;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => showField ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
    }
}