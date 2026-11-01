//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class AnimationPathEditor : EditorWindow
    {
        /// <summary>
        ///
        /// </summary>
        protected string oldPathPart = "";

        /// <summary>
        ///
        /// </summary>
        protected string newPathPart = "";


        /// <summary>
        ///
        /// </summary>
        protected readonly List<AnimationClip> clips = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly float cancelButtonWidth = 20;


        [MenuItem("DREditor/Utility/Animation Path Search and Replace")]
        public static void ShowWindow() => GetWindow<AnimationPathEditor>("Anim Path Editor");

        protected virtual void OnGUI()
        {
            GUILayout.Label("Update Animation Paths", EditorStyles.boldLabel);

            oldPathPart = EditorGUILayout.TextField("Old Name/Path:", oldPathPart);

            newPathPart = EditorGUILayout.TextField("New Name/Path:", newPathPart);

            EditorGUILayout.Space();

            AddClipsButton();

            DisplayClips();

            ReplacePathsButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddClipsButton()
        {
            if (GUILayout.Button("Add Selected Clips from Project Window"))
                foreach (Object obj in Selection.objects)
                    if (obj is AnimationClip clip && !clips.Contains(clip))
                        clips.Add(clip);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayClips()
        {
            for (int i = 0; i < clips.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                clips[i] = (AnimationClip)EditorGUILayout.ObjectField(clips[i], typeof(AnimationClip), false);

                if (GUILayout.Button("X", GUILayout.Width(cancelButtonWidth)))
                {
                    clips.RemoveAt(i);

                    i--;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ReplacePathsButton()
        {
            if (GUILayout.Button("Replace Paths in Clips"))
                ReplacePaths();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ReplacePaths()
        {
            foreach (AnimationClip clip in clips)
            {
                EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

                EditorCurveBinding[] objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

                Undo.RecordObject(clip, "Replace Animation Paths");

                ReplaceStandardCurves(clip, bindings);

                ReplaceReferenceCurves(clip, objectBindings);
            }

            UnityEngine.Debug.Log($"REPORT: Successfully updated {clips.Count} clips.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="bindings"></param>
        /// <remarks></remarks>
        protected virtual void ReplaceStandardCurves(AnimationClip clip, EditorCurveBinding[] bindings)
        {
            foreach (var binding in bindings)
            {
                if (binding.path.Contains(oldPathPart))
                {
                    string newPath = binding.path.Replace(oldPathPart, newPathPart);

                    AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);

                    AnimationUtility.SetEditorCurve(clip, binding, null);

                    EditorCurveBinding newBinding = binding;

                    newBinding.path = newPath;

                    AnimationUtility.SetEditorCurve(clip, newBinding, curve);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="bindings"></param>
        /// <remarks></remarks>
        protected virtual void ReplaceReferenceCurves(AnimationClip clip, EditorCurveBinding[] bindings)
        {
            foreach (var binding in bindings)
            {
                if (binding.path.Contains(oldPathPart))
                {
                    string newPath = binding.path.Replace(oldPathPart, newPathPart);

                    ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);

                    AnimationUtility.SetObjectReferenceCurve(clip, binding, null);

                    EditorCurveBinding newBinding = binding;

                    newBinding.path = newPath;

                    AnimationUtility.SetObjectReferenceCurve(clip, newBinding, keyframes);
                }
            }
        }
    }
}