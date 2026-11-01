// Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomClassEditor(typeof(StudentReactionCollection))]
    public class StudentReactionCollectionEditor : ClassEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        private const string ReactionsFieldName = "studentReactions";


        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            var parentEditor = GetFirstParameter<UnityEditor.Editor>();

            var serializedProperty = GetFirstParameter<SerializedProperty>();

            var eventIndex = GetFirstParameter<InterfaceEditorBase>().currentlyDisplayedEventIndex;

            var reactionsProp = serializedProperty.GetArrayElementAtIndex(eventIndex)
                                                 .FindPropertyRelative(ReactionsFieldName);

            var cache = parentEditor as INestedEditorCacheable;

            if (!ValidateContext(parentEditor, serializedProperty, cache))
                return;

            var editor = CreateReactionEditor(parentEditor, cache);

            DisplayClearEntriesButton(editor);

            editor.DisplayInterfaceInstances(reactionsProp, cache);

            UpdateCache(cache, editor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prop"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        protected virtual bool ValidateContext(UnityEditor.Editor parent, SerializedProperty prop, INestedEditorCacheable cache)
        {
            if (cache != null)
                return true;

            HandyFields.Label($"ALERT: Parent editor must inherit {nameof(INestedEditorCacheable)}", EditorStyles.helpBox);

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        protected virtual InterfaceEditor<IReactionEntry> CreateReactionEditor(UnityEditor.Editor parent, INestedEditorCacheable cache)
        {
            if (!cache.VisibilityCaches.TryGetValue(baseTarget, out bool isVisible))
                isVisible = true;

            cache.IndexCaches.TryGetValue(baseTarget, out int index);

            return new InterfaceEditor<IReactionEntry>(parent, ((StudentReactionCollection)baseTarget).studentReactions, isVisible, index);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="editor"></param>
        protected virtual void DisplayClearEntriesButton(InterfaceEditor<IReactionEntry> editor)
        {
            if (HandyFields.Button("Clear") && EditorUtility.DisplayDialog("Clear Entries", "Are you sure you want to clear all entries?",
                "Yes", "No"))
                editor.instances.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="editor"></param>
        protected virtual void UpdateCache(INestedEditorCacheable cache, InterfaceEditor<IReactionEntry> editor)
        {
            cache.VisibilityCaches[baseTarget] = editor.showDiaEvents;

            cache.IndexCaches[baseTarget] = editor.currentlyPickedIndex;
        }
    }
}

#endif