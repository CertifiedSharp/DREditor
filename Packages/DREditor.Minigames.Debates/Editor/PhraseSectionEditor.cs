//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PhraseSectionEditor<T> : PhraseSectionEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected int typeIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected List<Type> types;

        /// <summary>
        ///
        /// </summary>
        protected string[] choices;

        /// <summary>
        ///
        /// </summary>
        protected T currentInstance;


        protected PhraseSectionEditor(Color color, float endSpace) : base(color, endSpace) { }


        /// <summary>
        ///
        /// </summary>
        /// <param name="types"></param>
        /// <param name="choices"></param>
        protected abstract void UpdateTypeList(out List<Type> types, out string[] choices);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract string TypeDisplayName();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract string SectionLabel();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract string InstancesPropertyName();

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        protected abstract string GetInstanceDisplayName(T instance);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract List<T> TypeInstances();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> NamesOfInstances()
            => TypeInstances()?.Select(instance => GetInstanceDisplayName(instance));


        protected override string FoldoutLabel() => foldout || TypeInstances()?.Count == 0 ? SectionLabel() :
            SectionLabel() + ": \n\t- " + string.Join("\n\t- ", NamesOfInstances());


        protected override void Display()
        {
            CheckTypeList();

            DisplayTypeSections();

            GUILayout.Space(10);

            DisplayCreateToolbar();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckTypeList()
        {
            if (types == null || types.Count == 0)
                UpdateTypeList(out types, out choices);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTypeSections()
        {
            for (int i = 0; i < TypeInstances()?.Count; i++)
                DisplayTypeSection(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayTypeSection(int i)
        {
            currentInstance = TypeInstances()[i];

            using (new EditorGUILayout.HorizontalScope("Box"))
            {
                // Display instance with tinted Color for visibility.
                GUI.backgroundColor = Color.Lerp(color, color * originalGUIColor, 0.5f);

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUI.backgroundColor = originalGUIColor;

                    DisplayCurrentInstance();

                    GUILayout.Space(5);
                }

                DisplayRemoveButton();
            }

            HandySplitters.UISplitterCustom(Color.gray, 1, 5, 10);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCurrentInstance()
        {
            if (currentInstance != null && !DisplayCustomClassEditor())
                DisplayProperty();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool DisplayCustomClassEditor();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayProperty()
        {
            SerializedProperty nestedProp = phraseProp
                    .FindPropertyRelative(InstancesPropertyName())
                    .GetArrayElementAtIndex(TypeInstances().IndexOf(currentInstance));

            try // Added because the unity internal code would run an error when clicking on some properties. Works normally otherwise.
            {
                EditorGUILayout.PropertyField(nestedProp, new GUIContent(GetInstanceDisplayName(currentInstance)), true);
            }
            catch { }

            nestedProp.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRemoveButton()
        {
            if (GUILayout.Button("X", GUILayout.MaxWidth(20)) &&
                    EditorUtility.DisplayDialog($"Remove {TypeDisplayName()}",
                    $"Are you sure you want to delete this {TypeDisplayName()}?",
                        "Yes", "No"))
                TypeInstances()?.Remove(currentInstance);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCreateToolbar()
        {
            using (new EditorGUILayout.HorizontalScope("IN Title"))
            {
                typeIndex = EditorGUILayout.Popup(typeIndex, choices,
                    HandyFields.CalculateWidthOption(choices[typeIndex]));

                if (HandyFields.Button("Create " + TypeDisplayName()))
                    CreateTypeInstance();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateTypeInstance()
        {
            if (TypeConstructorsAreValid())
                TypeInstances()?.Add((T)Activator.CreateInstance(types[typeIndex]));
            else
                UnityEngine.Debug.LogError($"ALERT: Type {types[typeIndex]} must have a parameterless constructor!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool TypeConstructorsAreValid()
            => types[typeIndex].GetConstructors().Any(constructor => constructor.GetParameters().Length == 0);
    }
}