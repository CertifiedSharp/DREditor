//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public class MassDialogueEdit : ScriptableWizard
    {
        /// <summary>
        /// 
        /// </summary>
        protected SerializedObject serializedObject;

        /// <summary>
        /// 
        /// </summary>
        [SerializeReference]
        protected List<IEndEvent> endEvents = new();

        /// <summary>
        /// 
        /// </summary>
        protected int endEventIndex;


        /// <summary>
        /// 
        /// </summary>
        protected readonly InterfaceEditor<IEndEvent> EndEventEditor = new();


        /// <summary>
        /// 
        /// </summary>
        [MenuItem("DREditor/Dialogue/Mass Edit Dialogues")]
        public static void CreateWizard() => DisplayWizard<MassDialogueEdit>("Edit Dialogue", "Apply to Dialogues");


        protected override bool DrawWizardGUI()
        {
            try
            {
                DisplayEndEvents();
            }
            catch (ArgumentException) { } // Skipping stupid argument exception about geting control during repaint.

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayEndEvents()
        {
            serializedObject ??= new(this);

            EndEventEditor.Initialize(null, endEvents, true, endEventIndex);

            EndEventEditor.DisplayInterfaceInstances(serializedObject.FindProperty(nameof(endEvents)));

            endEvents = EndEventEditor.instances;

            endEventIndex = EndEventEditor.currentlyPickedIndex;
        }


        protected virtual void OnWizardCreate()
        {
            ReplaceSelectedObjectsEndEvents();

            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ReplaceSelectedObjectsEndEvents()
        {
            foreach (UnityEngine.Object selectedObject in Selection.objects)
                ReplaceContainer(selectedObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedObject"></param>
        protected virtual void ReplaceContainer(UnityEngine.Object selectedObject)
        {
            if (selectedObject is IEndEventsContainer container)
                ReplaceEndEventsFor(selectedObject, container);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedObject"></param>
        /// <param name="container"></param>
        protected virtual void ReplaceEndEventsFor(UnityEngine.Object selectedObject, IEndEventsContainer container)
        {
            container.EndEvents.Clear();

            foreach (var endEvent in endEvents)
                container.EndEvents.Add(endEvent);

            EditorUtility.SetDirty(selectedObject);
        }
    }
}
#endif