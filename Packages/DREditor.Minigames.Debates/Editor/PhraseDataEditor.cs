//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
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
    public class PhraseDataEditor : PhraseBaseDataEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly TextByArrayEditor wordEditor = new("Phrase");

        /// <summary>
        ///
        /// </summary>
        protected Action<PhraseBase>[] customDataDisplays;


        public PhraseDataEditor(Color color, float endSpace, params Action<PhraseBase>[] customDataDisplays) : base(color, endSpace) 
            => this.customDataDisplays = customDataDisplays;


        /// <inheritdoc/>
        protected override void DisplayTextField()
        {
            if (currPhrase is Phrase phrase)
                panel.WordArray = wordEditor.DisplayWordEditor(phrase, panel.WordArray, panel.Text);
            else
                base.DisplayTextField();
        }

        /// <inheritdoc/>
        protected override void DisplayPhraseData()
        {
            DisplayCustomData();

            DisplayFuckUp();

            base.DisplayPhraseData();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCustomData()
        {
            foreach (var action in customDataDisplays)
                action?.Invoke(currPhrase);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFuckUp()
        {
            if (currPhrase is Phrase phrase && phrase.Has<HittableModifierBase>())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    HandyFields.Label("Fuck up Dialogue: ");

                    phrase.SetFuckUpDialogue(HandyFields.UnityField(phrase.fuckUpDialogue));
                }
            }
        }
    }
}