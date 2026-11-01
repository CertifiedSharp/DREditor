//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues.Events;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.DialogueEventManaging.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(DialogueEventExcluder))]
    public class DialogueEventExcluderEditor : UnityEditor.Editor
    {
        /// <summary>
        ///
        /// </summary>
        protected DialogueEventExcluder excluder;

        /// <summary>
        ///
        /// </summary>
        protected List<Type> types;

        /// <summary>
        ///
        /// </summary>
        protected string[] choices;

        /// <summary>
        ///
        /// </summary>
        protected int diaEventIndex;

        /// <summary>
        ///
        /// </summary>
        protected int typeCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected int eventTypesIndex = 0;


        protected virtual void OnEnable()
        {
            excluder = (DialogueEventExcluder)target;

            UpdateDialogueEventList();

            typeCount = excluder.exclusionTypes.Count;
        }

        /// <summary>
        ///
        /// </summary>
        protected void UpdateDialogueEventList()
        {
            DialogueEventList.UpdateTypeList(out types, out choices);
        }

        /// <inheritdoc/>
        public override void OnInspectorGUI() => DisplayEventTypes();

        /// <summary>
        ///
        /// </summary>
        protected void DisplayEventTypes()
        {
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                TrackTypeCount();

                DisplayExclusionTypesPanel();

                AddButtonPanel();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void TrackTypeCount()
        {
            if (excluder.exclusionTypes.Count != typeCount)
                EditorUtility.SetDirty(excluder);

            typeCount = excluder.exclusionTypes.Count;
        }

        /// <summary>
        ///
        /// </summary>
        protected void DisplayExclusionTypesPanel()
        {
            for (int index = 0; index < excluder.exclusionTypes.Count; index++)
            {
                eventTypesIndex = index;

                EditorGUILayout.BeginHorizontal("Box");

                DisplayExclustionTypes();

                if (RemoveButtonPanel())
                    continue;

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void DisplayExclustionTypes()
        {
            using (new EditorGUILayout.VerticalScope())
                if (excluder.exclusionTypes[eventTypesIndex] != null)
                    DisplayExclusionType();
                else
                    DisplayNullDialogueEventHelpBox();
        }

        /// <summary>
        ///
        /// </summary>
        protected void DisplayExclusionType()
        {
            EditorGUILayout.LabelField(excluder.exclusionTypes[eventTypesIndex]?.ToString(), EditorStyles.largeLabel);

            HandySplitters.UISplitterCustom(Color.gray, 1, 5, 10);
        }

        /// <summary>
        ///
        /// </summary>
        protected void DisplayNullDialogueEventHelpBox()
        {
            EditorGUILayout.HelpBox("Unsupported/Null Dialogue Event.\n" +
                                  "This may have been broken by an update. " +
                                  "Please delete this using the - button on the right side of this error " +
                                  "and reapply your event. If still isn't fixed, post an issue on GitHub.", MessageType.Error, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool RemoveButtonPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(20)))
            {
                if (GUILayout.Button(new GUIContent("-", "Delete Dialogue Event." +
                    "\nWarning: this is non-recoverable. Using undo won't do a thing."),
                    GUILayout.Width(20)))
                {
                    excluder.exclusionTypes.Remove(excluder.exclusionTypes[eventTypesIndex]);

                    EditorUtility.SetDirty(excluder);

                    Repaint();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddButtonPanel()
        {
            EditorGUILayout.BeginHorizontal("Box");

            AddDropdown();

            AddButton();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///
        /// </summary>
        protected void AddDropdown() => diaEventIndex = EditorGUILayout.Popup(diaEventIndex, choices);

        /// <summary>
        ///
        /// </summary>
        protected void AddButton()
        {
            if (GUILayout.Button("Add Dialogue Event"))
            {
                excluder.exclusionTypes.Add((IDialogueEvent)Activator.CreateInstance(types[diaEventIndex]));

                EditorUtility.SetDirty(excluder);
            }
        }
    }
}
#endif