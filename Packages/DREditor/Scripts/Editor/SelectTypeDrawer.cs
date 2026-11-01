//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomPropertyDrawer(typeof(SelectTypeAttribute))]
    public class SelectTypeDrawer : PropertyDrawer
    {
        /// <summary>
        ///
        /// </summary>
        protected Rect lineRect;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty property;


        /// <summary>
        ///
        /// </summary>
        private List<Type> cachedTypes;

        /// <summary>
        ///
        /// </summary>
        private string[] typeNames;


        /// <summary>
        ///
        /// </summary>
        const string AttributeInvalidMessage = "Use [SelectType] only on [SerializeReference] fields!";

        /// <summary>
        ///
        /// </summary>
        const string PropertyNullDisplay = "None (Null)";

        /// <summary>
        ///
        /// </summary>
        const string ArrayFieldName = "Array";


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;

            if (AttributeInvalid(position, label))
                return;

            CacheTypes();

            UpdateLineRect(position);

            if (DisplayValidDropdown(label))
                DrawPropertyFields();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        protected virtual bool AttributeInvalid(Rect position, GUIContent label)
        {
            bool result = property.propertyType != SerializedPropertyType.ManagedReference;

            if (result)
                EditorGUI.LabelField(position, label.text, AttributeInvalidMessage);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CacheTypes()
        {
            if (cachedTypes != null)
                return;

            Type fieldType = GetFieldType(property);

            cachedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => fieldType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract &&
                t.IsDefined(typeof(SerializableAttribute), true))
                .ToList();

            typeNames = cachedTypes.Select(t => t.Name).Prepend(PropertyNullDisplay).ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        protected virtual void UpdateLineRect(Rect position)
        {
            lineRect = position;

            lineRect.height = EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        protected virtual bool DisplayValidDropdown(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            label.tooltip = GetTooltipText();

            int currentSelectedIndex = GetCurrentIndex(property);

            int[] optionValues = new int[typeNames.Length];

            GUIContent[] displayOptions = new GUIContent[typeNames.Length];

            for (int i = 0; i < typeNames.Length; i++)
            {
                optionValues[i] = i;

                displayOptions[i] = new(typeNames[i]);
            }

            int newIndex = EditorGUI.IntPopup(lineRect, label, currentSelectedIndex, displayOptions, optionValues);

            if (EditorGUI.EndChangeCheck())
            {
                if (newIndex == 0)
                    property.managedReferenceValue = null;
                else
                    property.managedReferenceValue = Activator.CreateInstance(cachedTypes[newIndex - 1]);

                property.serializedObject.ApplyModifiedProperties();

                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTooltipText()
        {
            TooltipAttribute tooltip = fieldInfo.GetCustomAttribute<TooltipAttribute>();

            if (tooltip != null)
                return tooltip.tooltip;

            return ((SelectTypeAttribute)attribute).toolTipText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <returns>
        ///
        /// </returns>
        protected virtual int GetCurrentIndex(SerializedProperty property)
        {
            string currentFullType = property.managedReferenceFullTypename;

            if (string.IsNullOrEmpty(currentFullType))
                return 0;

            string[] parts = currentFullType.Split(' ');

            string typeName = parts.Length > 1 ? parts[1] : parts[0];

            int index = cachedTypes.FindIndex(t => t.FullName == typeName || t.AssemblyQualifiedName == currentFullType);

            return index != -1 ? index + 1 : 0;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawPropertyFields()
        {
            if (property.managedReferenceValue != null)
                DrawNestedFields();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawNestedFields()
        {
            EditorGUI.indentLevel++;

            SerializedProperty child = property.Copy();

            Rect childRect = lineRect;

            int depth = child.depth;

            while (child.NextVisible(!child.isArray) && child.depth > depth)
            {
                childRect.y += childRect.height + EditorGUIUtility.standardVerticalSpacing;

                childRect.height = EditorGUI.GetPropertyHeight(child, true);

                EditorGUI.PropertyField(childRect, child, true);
            }

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;

            if (property.managedReferenceValue != null)
            {
                SerializedProperty child = property.Copy();

                int depth = child.depth;

                while (child.NextVisible(true) && child.depth > depth)
                    totalHeight += EditorGUI.GetPropertyHeight(child, false) + EditorGUIUtility.standardVerticalSpacing;
            }

            return totalHeight;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected virtual Type GetFieldType(SerializedProperty property)
        {
            string[] path = property.propertyPath.Split('.');

            Type type = property.serializedObject.targetObject.GetType();

            string currentPath = string.Empty;

            for (int i = 0; i < path.Length; i++)
            {
                string fieldName = path[i];

                currentPath = string.IsNullOrEmpty(currentPath) ? fieldName : currentPath + "." + fieldName;

                if (fieldName == ArrayFieldName)
                {
                    i++;

                    if (i >= path.Length)
                        break;

                    currentPath += "." + path[i];

                    if (type.IsGenericType)
                        type = type.GetGenericArguments()[0];
                    else if (type.IsArray)
                        type = type.GetElementType();
                }
                else
                {
                    FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    if (field != null)
                        type = field.FieldType;
                }

                bool isFinalSegment = i == path.Length - 1;

                // Whatever kind of segment we just resolved (array element or plain field),
                // if it's a managed reference and it's not the field we're currently drawing,
                // swap the declared type out for its actual runtime type so the *next*
                // segment's reflection lookup has something real to search against.
                if (!isFinalSegment)
                {
                    Type concreteType = ResolveManagedReferenceType(property, currentPath);

                    if (concreteType != null)
                        type = concreteType;
                }
            }

            return type;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual Type ResolveManagedReferenceType(SerializedProperty rootProperty, string elementPath)
        {
            try
            {
                SerializedProperty elementProperty = rootProperty.serializedObject.FindProperty(elementPath);

                if (elementProperty == null)
                    return null;

                return GetManagedReferenceType(elementProperty.managedReferenceFullTypename);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="managedReferenceFullTypename"></param>
        /// <returns></returns>
        protected virtual Type GetManagedReferenceType(string managedReferenceFullTypename)
        {
            if (string.IsNullOrEmpty(managedReferenceFullTypename))
                return null;

            string[] parts = managedReferenceFullTypename.Split(' ');

            if (parts.Length != 2)
                return null;

            string assemblyName = parts[0];

            string typeFullName = parts[1];

            try
            {
                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == assemblyName);

                return assembly?.GetType(typeFullName);
            }
            catch
            {
                return null;
            }
        }
    }
}