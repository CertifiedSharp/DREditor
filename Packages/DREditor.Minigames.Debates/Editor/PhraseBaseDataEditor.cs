//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PhraseBaseDataEditor : PhraseSectionEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        private const string PhraseTextPropertyName = "phraseText";


        public PhraseBaseDataEditor(Color color, float endSpace) : base(color, endSpace) { }


        /// <inheritdoc/>
        protected override string FoldoutLabel() => "Phrase Data";

        /// <inheritdoc/>
        protected override void DisplayFoldout()
        {
            base.DisplayFoldout();

            DisplayTextPreview();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTextPreview()
        {
            string labelText = HandyFields.GetPreviewText(currPhrase.Text);

            labelText = foldout ? labelText : "\t" + labelText;

            GUILayout.Label(labelText, EditorStyles.boldLabel);
        }


        /// <inheritdoc/>
        protected override void Display() => DisplayPhraseData();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhraseData()
        {
            DisplaySize();

            DisplayTextField();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySize()
            => currPhrase.PhraseSize = HandyFields.FloatField("Size of Phrase: ", currPhrase.PhraseSize, 30);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTextField()
        {
            if (phraseProp?.FindPropertyRelative(PhraseTextPropertyName) is SerializedProperty textProperty)
                EditorGUILayout.PropertyField(textProperty, true);
        }
    }
}