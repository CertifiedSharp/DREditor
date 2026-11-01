//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.DialogueEventManaging.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(LoadDialogueEventExcluder))]
    public class LoadDialogueEventExcluderEditor : DialogueEventExcluderEditor
    {
        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            DisplayIsLoading();

            base.OnInspectorGUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected void DisplayIsLoading()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Is Loading File: ", GUILayout.Width(100));

                ((LoadDialogueEventExcluder)excluder)
                    .SetIsLoadingFile(HandyFields.UnityField(((LoadDialogueEventExcluder)excluder).isLoadingFile, 250, 25));
            }
        }
    }
}
#endif