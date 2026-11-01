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
    public class PanelPhrasesEditor<T> where T : Panel, new()
    {
        /// <summary>
        ///
        /// </summary>
        protected int phraseTypeIndex;

        /// <summary>
        ///
        /// </summary>
        protected List<Type> _phraseTypes;

        /// <summary>
        ///
        /// </summary>
        protected string[] phraseChoices;

        /// <summary>
        ///
        /// </summary>
        protected T panel;

        /// <summary>
        ///
        /// </summary>
        protected IDebateEditor debateEditor;

        /// <summary>
        ///
        /// </summary>
        protected PhraseBase serializedPhrase;

        /// <summary>
        ///
        /// </summary>
        protected SerializedProperty phraseProp;

        /// <summary>
        ///
        /// </summary>
        protected readonly PhrasePresetEditor presetEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly PhraseBaseEditor fallbackPhraseEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<PhraseBase, object> editorInstances;


        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="debateEditor"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void Initialize(T panel, IDebateEditor debateEditor)
        {
            this.panel = panel ?? throw new ArgumentNullException(nameof(panel));

            this.debateEditor = debateEditor ?? throw new ArgumentNullException(nameof(debateEditor));

            presetEditor.Initialize();

            if (panel.Phrases.Count != 0)
            {
                editorInstances ??= new Dictionary<PhraseBase, object>();

                CreatePhraseEditors();

                RemovePhraseEditors();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreatePhraseEditors()
        {
            foreach (var phrase in panel.Phrases)
                if (!editorInstances.ContainsKey(phrase))
                {
                    var editor = ClassEditorFactory.CreateEditorInstance(phrase);

                    if (editor != null)
                        editorInstances[phrase] = editor;
                }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemovePhraseEditors()
        {
            var phrasesToRemove = editorInstances.Keys
                    .Where(phrase => !panel.Phrases.Contains(phrase))
                    .ToList();

            foreach (var phrase in phrasesToRemove)
                editorInstances.Remove(phrase);
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void DisplayPhrases()
        {
            if (panel.Phrases.Count != 0 && !debateEditor.IsFocusingPhrase())
                for (int i = 0; i < panel.Phrases.Count; i++)
                    DisplayPhrase(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        public virtual void DisplayPhrase(int i)
        {
            if (DisplayPhraseHeader(i) && PhraseCanBeDisplayed(i))
                DisplayPhraseEditor();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool DisplayPhraseHeader(int i)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope("Box"))
                    if (!DisplayPhraseTitle(i))
                        return false;

                if (presetEditor.DatabaseAvailable())
                    presetEditor.DisplayPreset(panel.Phrases[i]);
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool DisplayPhraseTitle(int i)
        {
            string typeLabel = $"Phrase Type: {panel.Phrases[i].GetType().Name}";

            using (new EditorGUILayout.HorizontalScope(HandyFields.CalculateWidthOption(typeLabel)))
            {
                HandyFields.Label($"Phrase {i + 1} ");

                DisplayFocusButton(i);

                EditorGUILayout.Space(PanelEditorConstants.EditorSpacing / PanelEditorConstants.DivideSpacing);

                if (DisplayRemoveButton(i))
                    return false;
            }

            HandyFields.Label(typeLabel);

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayFocusButton(int i)
        {
            using (new EditorGUI.DisabledScope(debateEditor.IsFocusingPhrase()))
                if (GUILayout.Button("^", GUILayout.MaxWidth(PanelEditorConstants.EditorSpacing)))
                    debateEditor.SetFocusPhrase(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool DisplayRemoveButton(int i)
        {
            bool removeButtonClicked = GUILayout.Button("X", GUILayout.MaxWidth(PanelEditorConstants.EditorSpacing)) &&
                EditorUtility.DisplayDialog("Remove Phrase", "Are you sure you want to Delete this Phrase?",
                        "Yes", "No");

            bool removeCondition = removeButtonClicked && i < panel.Phrases.Count;

            if (removeCondition)
                panel.Phrases.Remove(panel.Phrases[i]);

            return removeCondition;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual bool PhraseCanBeDisplayed(int i)
        {
            if (panel == null || i < 0 || i >= panel.Phrases.Count)
                return false;

            var phrase = panel.Phrases[i];

            if (serializedPhrase != phrase || phraseProp == null)
            {
                phraseProp = debateEditor.GetPhraseProperty(phrase);

                serializedPhrase = phrase;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhraseEditor()
        {
            if (editorInstances != null && editorInstances.TryGetValue(serializedPhrase, out object editorInstance))
                ClassEditorFactory.UseCustomEditor(serializedPhrase, editorInstance, panel, debateEditor.EditPanelNum, phraseProp);
            else
                fallbackPhraseEditor.OnGUI(serializedPhrase, panel, debateEditor.EditPanelNum, phraseProp);
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void DisplayCreatePhraseToolbar()
        {
            if (panel == null || debateEditor.IsFocusingPhrase())
                return;

            using (new GUILayout.HorizontalScope("IN Title"))
            {
                EditorGUILayout.BeginHorizontal();

                DisplayAddFromPresetButton();

                GUILayout.FlexibleSpace();

                DisplayAddFromClassButton();

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddFromPresetButton()
        {
            if (presetEditor.HasPresets() && presetEditor.DisplaySpawnPreset(out PhraseBase preset))
                panel.Phrases.Add(preset);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddFromClassButton()
        {
            if (_phraseTypes == null || _phraseTypes.Count == 0)
                PhraseTypeList.UpdateTypeList(out _phraseTypes, out phraseChoices);

            phraseTypeIndex = EditorGUILayout.Popup(phraseTypeIndex, phraseChoices,
                HandyFields.CalculateWidthOption(phraseChoices[phraseTypeIndex]));

            if (HandyFields.Button("Add From Class"))
                panel.Phrases.Add((PhraseBase)Activator.CreateInstance(_phraseTypes[phraseTypeIndex]));
        }
    }
}