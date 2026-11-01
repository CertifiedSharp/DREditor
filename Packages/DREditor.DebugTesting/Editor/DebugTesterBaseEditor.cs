//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DREditor.DebugTesting.Editor
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DebugTesterBaseEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected bool eventFoldout = false;


        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            if (!PassInitialization())
                return;

            serializedObject.Update();

            CreateForm();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool PassInitialization();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            SaveChangesButton();

            DisplayEventFields();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SaveChangesButton()
        {
            if (GUILayout.Button("Save Changes", GUILayout.Width(100)))
                EditorUtility.SetDirty(target);

            GUILayout.Space(20);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayEventFields()
        {
            eventFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(eventFoldout, "Initialized Objects and Events");

            if (eventFoldout)
                DrawDefaultInspector();

            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(10);
        }
    }
}
#endif