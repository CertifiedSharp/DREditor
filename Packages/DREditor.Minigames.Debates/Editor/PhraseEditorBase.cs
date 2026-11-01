//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class PhraseEditorBase<T> : ClassEditorTypeBase<T> where T : PhraseBase
    {
        /// <summary>
        ///
        /// </summary>
        protected T currPhrase;

        /// <summary>
        ///
        /// </summary>
        protected Panel panel;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty phraseProp;

        /// <summary>
        ///
        /// </summary>
        protected int editPanelNum;

        /// <summary>
        ///
        /// </summary>
        protected Color originalGUIColor;


        /// <summary>
        ///
        /// </summary>
        private List<PhraseSectionEditorBase> _sections;

        /// <summary>
        ///
        /// </summary>
        protected List<PhraseSectionEditorBase> Sections
        {
            get
            {
                _sections ??= InitializeSections();

                return _sections;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract List<PhraseSectionEditorBase> InitializeSections();


        public override void OnGUI(object target, params object[] objects)
        {
            try
            {
                InitializeParameters(objects);
            }
            catch
            {
                return;
            }

            base.OnGUI(target, objects);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objects"></param>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void InitializeParameters(params object[] objects)
        {
            if (objects == null || objects.Length < 3)
                throw new ArgumentException("Expected at least 3 parameters", nameof(objects));

            InitializeParameter(objects[0], out panel);

            InitializeParameter(objects[1], out editPanelNum);

            InitializeParameter(objects[2], out phraseProp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="parameterObject"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void InitializeParameter<T2>(object parameter, out T2 parameterObject)
        {
            if (parameter == null) 
                throw new ArgumentNullException(nameof(parameter));

            if (parameter is not T2 castedObject)
                throw new ArgumentException($"Phrase Editor expected: {typeof(T2).Name}, got {parameter?.GetType().Name}");

            parameterObject = castedObject;
        }

        protected override void OnInspectorGUI()
        {
            currPhrase = target;

            originalGUIColor = GUI.backgroundColor;

            if (phraseProp != null)
                DisplaySections();
            else
                HandyFields.Label("ALERT: Failed to display sections. " +
                    "\nIt is likely that your debates lines/panels do not inherit from the \"Line\" class.");

            HandySplitters.UISplitter();

            GUI.backgroundColor = originalGUIColor;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySections()
        {
            foreach (var section in Sections)
            {
                try
                {
                    section.DisplayPhraseSection(currPhrase, panel, phraseProp);
                }
                catch (Exception exception)
                {
                    EditorGUILayout.HelpBox($"Error in {section.GetType().Name}: {exception.Message}",
                        MessageType.Error);

                    UnityEngine.Debug.LogException(exception);
                }
            }
        }
    }
}