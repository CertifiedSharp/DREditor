//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.EventObjects;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.UI.Options.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(OptionBuilder))]
    public class OptionBuilderEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly string typePropName = "typeName";

        /// <summary>
        ///
        /// </summary>
        protected readonly string optionParentName = "optionParent";

        /// <summary>
        ///
        /// </summary>
        protected readonly string menuGroupName = "menuGroup";

        /// <summary>
        ///
        /// </summary>
        protected readonly string optionAssetsName = "optionAssets";

        /// <summary>
        ///
        /// </summary>
        protected readonly string optionAssetName = "optionAsset";

        /// <summary>
        ///
        /// </summary>
        protected readonly string optionTypesName = "optionTypes";

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty optionParentProp;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty menuGroupProp;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty optionAssetsProp;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty optionTypesProp;

        /// <summary>
        ///
        /// </summary>
        protected readonly float spacing = 10;

        /// <summary>
        ///
        /// </summary>
        protected readonly float instanceSpacing = 2;

        /// <summary>
        ///
        /// </summary>
        protected readonly int boxOffsetAmount = 8;

        /// <summary>
        ///
        /// </summary>
        protected bool typesFoldout = true;

        /// <summary>
        ///
        /// </summary>
        protected bool instancesFoldout = true;

        /// <summary>
        ///
        /// </summary>
        protected GUIStyle boxStyle;

        /// <summary>
        ///
        /// </summary>
        protected GUIStyle headerStyle;


        protected virtual void OnEnable()
        {
            optionParentProp = serializedObject.FindProperty(optionParentName);

            menuGroupProp = serializedObject.FindProperty(menuGroupName);

            optionAssetsProp = serializedObject.FindProperty(optionAssetsName);

            optionTypesProp = serializedObject.FindProperty(optionTypesName);
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            InitializeStyles();

            DrawSimpleFields();

            GUILayout.Space(spacing);

            DrawOptionInstancesSection();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeStyles()
        {
            if (boxStyle != null)
                return;

            RectOffset boxOffset = new(boxOffsetAmount, boxOffsetAmount, boxOffsetAmount, boxOffsetAmount);

            boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = boxOffset,
                margin = boxOffset
            };

            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 11
            };
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawSimpleFields()
        {
            EditorGUILayout.PropertyField(optionParentProp, new GUIContent("Option Parent",
                "The parent transform under which all spawned option prefabs will be placed."), true);

            EditorGUILayout.PropertyField(menuGroupProp, new GUIContent("Menu Group",
                "The menu group that spawned option selectables will be registered to."), true);

            EditorGUILayout.PropertyField(optionTypesProp, new GUIContent("Option Types",
                "The list of available option type definitions mapping type names to their corresponding prefabs."), true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawOptionInstancesSection()
        {
            instancesFoldout = DrawSectionHeader("Option Instances", instancesFoldout, optionAssetsProp.arraySize);

            if (!instancesFoldout)
                return;

            DrawInstances();

            EditorGUILayout.Space(instanceSpacing);

            AddInstanceButton();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <param name="foldout"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected virtual bool DrawSectionHeader(string label, bool foldout, int count)
        {
            Rect headerRect = GUILayoutUtility.GetRect(0, spacing * instanceSpacing, GUILayout.ExpandWidth(true));

            return EditorGUI.Foldout(headerRect, foldout, $"  {label}  ({count})", true, EditorStyles.foldoutHeader);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawInstances()
        {
            string[] typeNameOptions = BuildTypeNameOptions();

            for (int i = 0; i < optionAssetsProp.arraySize; i++)
            {
                if (DrawInstanceElement(i, typeNameOptions))
                    break;

                EditorGUILayout.Space(instanceSpacing);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string[] BuildTypeNameOptions()
        {
            var names = new List<string> { "(None)" };

            for (int i = 0; i < optionTypesProp.arraySize; i++)
            {
                var property = optionTypesProp.GetArrayElementAtIndex(i).FindPropertyRelative(typePropName);

                if (!string.IsNullOrWhiteSpace(property?.stringValue))
                    names.Add(property.stringValue);
            }

            return names.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="typeNameOptions"></param>
        /// <returns></returns>
        protected virtual bool DrawInstanceElement(int index, string[] typeNameOptions)
        {
            SerializedProperty element = optionAssetsProp.GetArrayElementAtIndex(index);

            SerializedProperty optionAssetProp = element.FindPropertyRelative(optionAssetName);

            using (new EditorGUILayout.VerticalScope(boxStyle))
            {
                if (DrawInstanceHeader(optionAssetProp, index))
                    return true;

                DrawTypeNameDropdown(element, typeNameOptions);

                HandyEditor.DrawFilteredOptionAssetField<OptionEventWindow, IOptionEvent>(serializedObject, optionAssetProp);
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="optionAssetProp"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual bool DrawInstanceHeader(SerializedProperty optionAssetProp, int index)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"Instance {index}", headerStyle);

                if (HandyFields.SkinnyButton("X") && optionAssetProp.objectReferenceValue != null)
                {
                    optionAssetsProp.DeleteArrayElementAtIndex(index);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        /// <param name="typeNameOptions"></param>
        protected virtual void DrawTypeNameDropdown(SerializedProperty element, string[] typeNameOptions)
        {
            SerializedProperty typeNameProp = element.FindPropertyRelative(typePropName);

            int nameIndex = Mathf.Max(0, Array.IndexOf(typeNameOptions, typeNameProp.stringValue));

            int next = EditorGUILayout.Popup(new GUIContent("Type Name", "Select from the Option Types defined above."),
                nameIndex, typeNameOptions);

            typeNameProp.stringValue = next == 0 ? string.Empty : typeNameOptions[next];

            if (next == 0 && !string.IsNullOrEmpty(typeNameProp.stringValue))
                EditorGUILayout.HelpBox($"Type \"{typeNameProp.stringValue}\" not found in Option Types.",
                    MessageType.Warning);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void AddInstanceButton()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (HandyFields.Button("Add Instance"))
                    optionAssetsProp.InsertArrayElementAtIndex(optionAssetsProp.arraySize);
            }
        }


        /// <summary>
        ///
        /// </summary>
        sealed class OptionEventWindow : SimpleFilteredSOWindow<IOptionEvent> { }
    }
}
#endif