// Original Author: KHeartz
// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(StudentReaction))]
    public class StudentReactionEditor : ClassEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected ReactionLevelDatabase reactionDatabase;

        /// <summary>
        ///
        /// </summary>
        protected readonly string VerticalBoxLayoutStyle = "Box";


        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            INestedEditorCacheable cache = GetFirstParameter<INestedEditorCacheable>();

            if ((bool)!cache?.TryGetData(out reactionDatabase))
                cache.AddData(DatabaseUtility.LoadDatabase<ReactionLevelDatabase>());

            if (reactionDatabase == null || cache == null)
                return;

            using (new EditorGUILayout.HorizontalScope(VerticalBoxLayoutStyle))
                DisplayReaction(baseTarget as StudentReaction);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reaction"></param>
        protected virtual void DisplayReaction(StudentReaction reaction)
        {
            DisplayStudent(reaction);

            DisplayReactionDropdown(reaction);

            EditorGUILayout.Space();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reaction"></param>
        protected virtual void DisplayStudent(StudentReaction reaction)
        {
            using (new EditorGUILayout.VerticalScope())
                reaction.Character = HandyFields.UnityField(reaction.Character);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reaction"></param>
        protected virtual void DisplayReactionDropdown(StudentReaction reaction)
        {
            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.FlexibleSpace();

                reaction.ReactionIndex = EditorGUILayout.Popup(reaction.ReactionIndex, reactionDatabase.GetReactionNames());

                GUILayout.FlexibleSpace();
            }
        }
    }
}

#endif