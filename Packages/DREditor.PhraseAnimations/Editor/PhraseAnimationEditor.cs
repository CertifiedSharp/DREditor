//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(PhraseAnimation))]
    public class PhraseAnimationEditor : ClassEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected List<Type> _types;

        /// <summary>
        ///
        /// </summary>
        protected string[] choices;

        /// <summary>
        ///
        /// </summary>
        protected PhraseAnimation target;

        /// <summary>
        ///
        /// </summary>
        protected AnimData anim;

        // Since this assembly is self contained, Phrase Animation Presets will come in a TBD future update.
        //protected readonly PhrasePresetEditor presetEditor = new();

        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            target = baseTarget as PhraseAnimation;

            DisplaySpawnData();

            if (target.animations.Count > 0)
                DisplayPhraseAnimations();

            DisplayAddAnimationButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpawnData()
        {
            target.SetSpawnPoint(HandyFields.Vector3Field("Spawn Point: ", target.spawnPoint, 200, 7));

            target.SetSpawnAngle(HandyFields.Vector4Field("Spawn Angle: ", target.spawnAngle, 200, 7));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhraseAnimations()
        {
            for (int i = 0; i < target.animations.Count; i++)
                DisplayPhraseAnimation(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayPhraseAnimation(int i)
        {
            anim = target.animations[i];

            // Since this assembly is self contained, Phrase Animation Presets will come in a TBD future update.
            //presetEditor.EditPhrasePreset(currPhrase);

            InitializeTypeList();

            using (new EditorGUILayout.VerticalScope("Box"))
            {
                EditorGUILayout.LabelField("Animation " + (i + 1) + ": ");

                DisplayPhraseAnimationType();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTypeList()
        {
            if (_types == null || _types.Count == 0)
                PhraseAnimationTypeList.UpdateTypeList(out _types, out choices);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhraseAnimationType()
        {
            DisplayPhraseAnimationTypeDropdown();

            DisplayReplaceAnimationButton();

            DisplayPhraseAnimationEditor();

            DisplaySpawnAnimationButton();

            DisplayRemoveAnimationButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhraseAnimationTypeDropdown()
        {
            if (anim.animationType != null &&
                _types[anim.typeIndex] != anim.animationType.GetType())
                anim.SetIndex(_types.IndexOf(anim.animationType.GetType()));

            anim.SetIndex(EditorGUILayout.Popup(anim.typeIndex, choices));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayReplaceAnimationButton()
        {
            if (anim.animationType != null &&
                _types[anim.typeIndex] != anim.animationType.GetType() &&
                GUILayout.Button("Replace Animation"))
                anim.SetType((IPhraseAnimationType)Activator.CreateInstance(_types[anim.typeIndex]));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPhraseAnimationEditor()
        {
            if (anim.animationType != null)
                ClassEditorFactory.DrawEditorFor(anim.animationType);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplaySpawnAnimationButton()
        {
            if (anim.animationType == null && GUILayout.Button("Spawn Animation"))
                anim.SetType((IPhraseAnimationType)Activator.CreateInstance(_types[anim.typeIndex]));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRemoveAnimationButton()
        {
            if (GUILayout.Button("Remove Animation") &&
                    EditorUtility.DisplayDialog("Remove Animation", "Are you sure you want to Delete this Animation?",
                        "Yes", "No"))
                target.animations.Remove(anim);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddAnimationButton()
        {
            if (GUILayout.Button("Add Animation"))
                target.animations.Add(new AnimData());
        }
    }
}
#endif