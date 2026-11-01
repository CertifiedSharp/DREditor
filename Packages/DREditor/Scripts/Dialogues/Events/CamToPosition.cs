//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct CTPTuple
    //{

    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CamToPosition : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        //public CTPTuple cTPTuple;
        public string objectName;
        public bool keepFocus;
        // This could probably be turned into a vector3 or transform, just uses GameObject.find which isn't 
        // great, but implementation is easier, I'll change this eventually
        public void TriggerDialogueEvent()
        {
            
            DialogueEventSystem.TriggerEvent("CamToPosition", this);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("GameObject's Name: ", GUILayout.Width(135));
                objectName = EditorGUILayout.TextField(objectName);
                
            }
            keepFocus = EditorFields.Option(keepFocus, "Keep Focused character: ", 150);
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Write the name of a game object in the scene to " +
                "use it's transform for the position to move the camera.", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif
        

    }
}
