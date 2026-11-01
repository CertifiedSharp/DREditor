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
    [CustomClassEditor(typeof(PartlyHitModifier))]
    public class PartlyHitEditor : ClassEditorTypeBase<PartlyHitModifier>
    {
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
        protected readonly TextByArrayEditor wordEditor = new("Hittable Text");


        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            panel = (Panel)parameters[0];

            currPhrase = (PhraseBase)parameters[1];

            HandyFields.Label(nameof(PartlyHitModifier), EditorStyles.boldLabel);

            GUILayout.Space(PanelEditorConstants.EditorSpacing / PanelEditorConstants.DivideSpacing);

            PartlyHittablePhrase();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PartlyHittablePhrase()
        {
            DisplayTextField();

            DisplayTextPreview();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTextField()
        {
            target.WordArray = wordEditor.DisplayWordEditor(target, target.WordArray, currPhrase.Text);

            if (target.WordCount == 0)
                if (panel.WordArray.Length > 0)
                    target.SetText(panel.WordArray[0]);
                else
                    target.SetText("");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTextPreview() => GUILayout.Label(HandyFields.GetPreviewText(target.Text), EditorStyles.boldLabel);
    }
}