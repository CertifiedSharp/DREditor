//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.Editor.Utility
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NestedClassEditorBase<T> : ClassEditorBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected UnityEditor.Editor parentEditor;

        /// <summary>
        /// 
        /// </summary>
        protected SerializedProperty serializedProperty;

        /// <summary>
        /// 
        /// </summary>
        protected INestedEditorCacheable cache;

        /// <summary>
        /// 
        /// </summary>
        protected int eventIndex;

        /// <summary>
        /// 
        /// </summary>
        protected SerializedProperty nestedProp;

        /// <summary>
        /// 
        /// </summary>
        protected InterfaceEditor<T> editor;


        /// <summary>
        /// 
        /// </summary>
        protected abstract string TargetFieldName { get; }

        /// <summary>
        /// 
        /// </summary>
        protected abstract List<T> Instances { get; }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool ClearInstancesButtonAllowed => false;


        /// <inheritdoc/>
        protected override void OnInspectorGUI()
        {
            InitializeContext();

            if (!ValidateContext())
                return;

            InitializeNestedEditor();

            DisplayClearInstancesButton();

            DisplayNestedEditor();

            UpdateCache();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeContext()
        {
            parentEditor = GetFirstParameter<UnityEditor.Editor>();

            serializedProperty = GetFirstParameter<SerializedProperty>();

            cache = parentEditor as INestedEditorCacheable;

            eventIndex = GetFirstParameter<InterfaceEditorBase>().currentlyDisplayedEventIndex;

            nestedProp = serializedProperty.GetArrayElementAtIndex(eventIndex).FindPropertyRelative(TargetFieldName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateContext()
        {
            if (cache != null)
                return true;

            HandyFields.Label($"ALERT: Parent editor must inherit {nameof(INestedEditorCacheable)}", EditorStyles.helpBox);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeNestedEditor() => editor = CreateNestedEditor();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual InterfaceEditor<T> CreateNestedEditor()
        {
            if (!cache.VisibilityCaches.TryGetValue(baseTarget, out bool isVisible))
                isVisible = true;

            cache.IndexCaches.TryGetValue(baseTarget, out int index);

            return new InterfaceEditor<T>(parentEditor, Instances, isVisible, index);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayClearInstancesButton()
        {
            if (ClearInstancesButtonAllowed && ConfirmedClearingInstances())
                ClearEditorInstances();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ConfirmedClearingInstances() =>
            HandyFields.Button("Clear") &&
            EditorUtility.DisplayDialog("Clear Instances", "Are you sure you want to clear all Instances?", "Yes", "No");

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ClearEditorInstances() => editor?.instances.Clear();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayNestedEditor() => editor?.DisplayInterfaceInstances(nestedProp, cache);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateCache()
        {
            if (editor == null)
                return;

            cache.VisibilityCaches[baseTarget] = editor.showDiaEvents;

            cache.IndexCaches[baseTarget] = editor.currentlyPickedIndex;
        }
    }
}
#endif