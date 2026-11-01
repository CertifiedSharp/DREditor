//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct CISTuple
    //{
    //    public string objectName;
    //    public bool isSelectable;
    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ChangeItemSelect : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        //public CISTuple cISTuple;


        public string objectName;
        public bool isSelectable;

        // This could probably be turned into a vector3 or transform, just uses GameObject.find which isn't 
        // great, but implementation is easier, I'll change this eventually
        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("ChangeItemSelect", this);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("GameObject's Name: ", GUILayout.Width(135));
                objectName = EditorGUILayout.TextField(objectName);

            }
            isSelectable = EditorFields.Option(isSelectable, "Is Selectable: ", 150);
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Change an Item Actor in the rooms selectability!", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif


    }
}
