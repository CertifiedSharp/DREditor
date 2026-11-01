//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InterfaceEditor<T> : InterfaceEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        public bool showDiaEvents;

        /// <summary>
        ///
        /// </summary>
        public int currentlyPickedIndex;

        /// <summary>
        ///
        /// </summary>
        public List<T> instances;


        /// <summary>
        ///
        /// </summary>
        protected readonly List<Type> _types;

        /// <summary>
        ///
        /// </summary>
        protected readonly string[] choices;

        /// <summary>
        ///
        /// </summary>
        protected UnityEditor.Editor parentEditor;


        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty eventListProperty;

        /// <summary>
        ///
        /// </summary>
        protected object[] parameters;


        /// <summary>
        ///
        /// </summary>
        const string PascalCaseRegex = @"(?<!^)(?=[A-Z])";

        /// <summary>
        ///
        /// </summary>
        const string SplitString = " ";



        public InterfaceEditor()
        {
            GenericTypeList<T>.UpdateTypeList(out _types, out choices);
        }

        public InterfaceEditor(UnityEditor.Editor parentEditor, List<T> diaEvents,
            bool showDiaEvents, int currentlyPickedIndex = -1) : this()
        {
            this.parentEditor = parentEditor;

            this.instances = diaEvents;

            this.showDiaEvents = showDiaEvents;

            if (currentlyPickedIndex != -1)
                this.currentlyPickedIndex = currentlyPickedIndex;

            
        }


        public virtual void Initialize(UnityEditor.Editor parentEditor, List<T> diaEvents,
            bool showDiaEvents, int currentlyPickedIndex = -1)
        {
            this.parentEditor = parentEditor;

            this.instances = diaEvents;

            this.showDiaEvents = showDiaEvents;

            if (currentlyPickedIndex != -1)
                this.currentlyPickedIndex = currentlyPickedIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventListProperty"></param>
        /// <param name="parameters"></param>
        public virtual void DisplayInterfaceInstances(SerializedProperty eventListProperty, params object[] parameters)
        {
            this.eventListProperty = eventListProperty;

            this.parameters = parameters;

            using (new EditorGUILayout.HorizontalScope("Box"))
            using (new EditorGUILayout.VerticalScope())
                DisplayInterfaceInstancesGUILayout();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayInterfaceInstancesGUILayout()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(false)))
                if (instances != null)
                    using (new EditorGUILayout.VerticalScope("Box"))
                        DisplayInterfaceInstancesFoldout();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayInterfaceInstancesFoldout()
        {
            EditorGUILayout.EndFoldoutHeaderGroup();

            showDiaEvents = EditorGUILayout.BeginFoldoutHeaderGroup(showDiaEvents, EvaluateErrorsEventHeader());

            if (showDiaEvents)
                DisplayAllInterfaceInstances();

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual GUIContent EvaluateErrorsEventHeader()
        {
            GUIContent content;

            int _errors = ErrorCheck();

            if (_errors > 0)
                content = new GUIContent($"{InterfaceDisplayNamePlural()} ({instances.Count}) Errors: {_errors}",
                    $"Show list of {InterfaceDisplayNamePlural()} to be used.");
            else
                content = new GUIContent($"{InterfaceDisplayNamePlural()} ({instances.Count})",
                    $"Show list of {InterfaceDisplayNamePlural()} to be used.");

            return content;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual int ErrorCheck()
        {
            int _foundErrors = 0;

            for (int d = 0; d < instances.Count; d++)
                if (instances[d] == null)
                    _foundErrors++;

            return _foundErrors;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAllInterfaceInstances()
        {
            for (int i = 0; i < instances.Count; i++)
            {
                currentlyDisplayedEventIndex = i;

                EditorGUILayout.BeginHorizontal("Box");

                using (new EditorGUILayout.VerticalScope())
                    ValidateDisplayInterfaceInstanceGUI();

                if (RepaintDisplayInterfaceInstanceFooter())
                    continue;

                EditorGUILayout.EndHorizontal();
            }

            AddInterfaceInstanceButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ValidateDisplayInterfaceInstanceGUI()
        {
            if (instances[currentlyDisplayedEventIndex] != null)
                DisplayValidInterfaceInstance();
            else
                EditorGUILayout.HelpBox($"Unsupported/Null {InterfaceDisplayName()}.\n" +
                  "This may have been broken by an update. Please delete this using the" +
                  " - button on the right side of this error " +
                  $"and reapply your {InterfaceDisplayName()}. " +
                  "If still isn't fixed, post an issue on GitHub or contact Sweden for support.", MessageType.Error, true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayValidInterfaceInstance()
        {
            EditorGUILayout.LabelField(instances[currentlyDisplayedEventIndex]?.GetType().Name, EditorStyles.largeLabel);

            HandySplitters.UISplitterCustom(Color.gray, 1, 5, 10);

            if (eventListProperty == null)
                UnityEngine.Debug.LogWarning("NOTICE: Event list property was null, contact Sweden for support.");

            if (!ClassEditorFactory.DrawEditorFor(instances[currentlyDisplayedEventIndex], parameters) && !HasCustomEditorProcess())
            {
                try // Added because the unity internal code would run an error when clicking on some properties. Works normally otherwise.
                {
                    eventListProperty.serializedObject.Update();

                    eventListProperty.serializedObject.ApplyModifiedProperties();

                    SerializedProperty nestedProp = eventListProperty.GetArrayElementAtIndex(currentlyDisplayedEventIndex);

                    nestedProp.serializedObject.Update();

                    // This ensures the child labels for child properties in the nested properties to display.
                    // Because for some stupid fucking reason I have to remind unity to calculate default proportions.
                    EditorGUIUtility.labelWidth = 0; 

                    EditorGUILayout.PropertyField(nestedProp, new GUIContent(instances[currentlyDisplayedEventIndex]?.GetType().Name), true);

                    nestedProp.serializedObject.ApplyModifiedProperties();
                }
                catch
                {
                    EditorGUILayout.LabelField("Failed", EditorStyles.largeLabel);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool HasCustomEditorProcess() => false;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool RepaintDisplayInterfaceInstanceFooter()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(20)))
                if (RemoveInterfaceInstanceButton())
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool RemoveInterfaceInstanceButton()
        {
            bool removeButtonPressed = GUILayout.Button(new GUIContent("-",
                        $"Delete {InterfaceDisplayName()}.\nALERT: this is non-recoverable. Using undo won't do a thing."),
                        GUILayout.Width(20));

            if (removeButtonPressed)
                OnRemoveButtonPressed();

            return removeButtonPressed;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRemoveButtonPressed()
        {
            instances.RemoveAt(currentlyDisplayedEventIndex);

            parentEditor.Repaint();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddInterfaceInstanceButton()
        {
            EditorGUILayout.BeginHorizontal("Box");

            currentlyPickedIndex = EditorGUILayout.Popup(currentlyPickedIndex, choices);

            if (GUILayout.Button("Add" + InterfaceDisplayName()))
                instances.Add((T)Activator.CreateInstance(_types[currentlyPickedIndex]));

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string InterfaceDisplayName() => Regex.Replace(typeof(T).Name, PascalCaseRegex, SplitString)[1..];

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string InterfaceDisplayNamePlural()
        {
            string displayName = InterfaceDisplayName();

            if (displayName[^1] == 'y')
                displayName = displayName[..^1] + "ie";

            return displayName + 's';
        }
    }
}
#endif