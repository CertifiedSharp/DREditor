//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomPropertyDrawer(typeof(PropertyButtonAttribute))]
    public class ClassButtonDrawer : PropertyDrawer
    {
        /// <summary>
        ///
        /// </summary>
        private const float Padding = 2f;

        /// <summary>
        ///
        /// </summary>
        private const float ButtonHeight = 20f;


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float fieldHeight = EditorGUI.GetPropertyHeight(property, label, true);

            return fieldHeight + ButtonHeight + Padding;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PropertyButtonAttribute buttonAttribute = (PropertyButtonAttribute)attribute;

            float fieldHeight = EditorGUI.GetPropertyHeight(property, label, true);

            Rect fieldRect = new (position.x, position.y, position.width, fieldHeight);

            Rect buttonRect = new (position.x, position.y + fieldHeight + Padding, position.width, ButtonHeight);

            EditorGUI.PropertyField(fieldRect, property, label, true);

            if (GUI.Button(buttonRect, buttonAttribute.buttonText))
            {
                object parentInstance = GetParentObjectOfProperty(property);

                if (parentInstance != null)
                {
                    MethodInfo method = FindMethod(parentInstance.GetType(), buttonAttribute.methodName);

                    if (method != null)
                    {
                        property.serializedObject.ApplyModifiedProperties();

                        method.Invoke(parentInstance, null);

                        property.serializedObject.Update();
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"ALERT: Method '{buttonAttribute.methodName}' not found on " +
                            $"{parentInstance.GetType().Name} or its base classes.");
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private MethodInfo FindMethod(Type type, string methodName)
        {
            while (type != null)
            {
                MethodInfo method = type.GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                if (method != null) 
                    return method;

                type = type.BaseType;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private object GetParentObjectOfProperty(SerializedProperty prop)
        {
            string path = prop.propertyPath.Replace(".Array.data[", "[");

            object obj = prop.serializedObject.targetObject;

            string[] elements = path.Split('.');

            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];

                if (element.Contains("["))
                {
                    string elementName = element[..element.IndexOf("[")];

                    int index = Convert.ToInt32(element[element.IndexOf("[")..].Replace("[", "").Replace("]", ""));

                    obj = GetValueImp(obj, elementName, index);
                }
                else
                    obj = GetValueImp(obj, element);
            }

            return obj;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object GetValueImp(object source, string name)
        {
            if (source == null) 
                return null;

            Type type = source.GetType();

            while (type != null)
            {
                FieldInfo f = type.GetField(name, 
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                if (f != null) 
                    return f.GetValue(source);

                PropertyInfo p = type.GetProperty(name, 
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (p != null) 
                    return p.GetValue(source, null);

                type = type.BaseType;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private object GetValueImp(object source, string name, int index)
        {
            if (GetValueImp(source, name) is not IEnumerable enumerable) 
                return null;

            IEnumerator enumerator = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
                if (!enumerator.MoveNext())
                    return null;

            return enumerator.Current;
        }
    }
}