//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class DREMonoBehaviourEditor : UnityEditor.Editor
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<BehaviourAttributeEditorBase> editors = new();


        protected virtual void OnEnable()
        {
            CreateAttributeEditors();

            InitializeAttributeEditors();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void CreateAttributeEditors()
        {
            if (BehaviourAttributeEditorList.GetList().Count == 0)
                BehaviourAttributeEditorList.UpdateList();

            var editorTypes = BehaviourAttributeEditorList
                .GetList();
            
            foreach (var type in editorTypes)
                editors.Add((BehaviourAttributeEditorBase)Activator.CreateInstance(type));

            editors = editors.OrderBy(editor => editor.Priority).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeAttributeEditors()
        {
            foreach (var editor in editors)
                editor.Initialize(target, targets);
        }


        public override void OnInspectorGUI()
        {
            DisplayAttributeEditors();

            DrawDefaultInspector();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayAttributeEditors()
        {
            foreach (var editor in editors)
                editor.Display();
        }
    }
}