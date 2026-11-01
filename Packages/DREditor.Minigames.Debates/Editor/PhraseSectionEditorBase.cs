//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class PhraseSectionEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected Color color;

        /// <summary>
        ///
        /// </summary>
        protected float endSpace;

        /// <summary>
        ///
        /// </summary>
        protected PhraseBase currPhrase;

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
        protected Color originalGUIColor;

        /// <summary>
        ///
        /// </summary>
        protected bool foldout = false;


        protected PhraseSectionEditorBase(Color color, float endSpace)
        {
            this.color = color;

            this.endSpace = endSpace;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="currPhrase"></param>
        /// <param name="panel"></param>
        /// <param name="phraseProp"></param>
        public virtual void DisplayPhraseSection(PhraseBase currPhrase, Panel panel, SerializedProperty phraseProp)
        {
            this.currPhrase = currPhrase;

            this.panel = panel;

            this.phraseProp = phraseProp;

            originalGUIColor = GUI.backgroundColor;

            GUI.backgroundColor = color;

            using (new EditorGUILayout.VerticalScope("Box"))
            {
                DisplayFoldoutHeader();

                GUI.backgroundColor = originalGUIColor;

                DisplayFoldout();
            }

            GUILayout.Space(endSpace);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFoldoutHeader()
        {
            GUIStyle labelStyle = new(EditorStyles.label)
            {
                wordWrap = true
            };

            string labelText = FoldoutLabel();

            float height = EditorStyles.label.CalcHeight(new GUIContent(labelText),
                EditorGUIUtility.currentViewWidth - 20);

            Rect labelRect = EditorGUILayout.GetControlRect(GUILayout.Height(height));

            Rect foldoutRect = new(labelRect.x, labelRect.y, 15, EditorGUIUtility.singleLineHeight);

            foldout = EditorGUI.Foldout(foldoutRect, foldout, GUIContent.none, true);

            EditorGUI.LabelField(labelRect, labelText, labelStyle);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract string FoldoutLabel();


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFoldout()
        {
            if (foldout)
                Display();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void Display();
    }
}