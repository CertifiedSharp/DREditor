//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public class OTuple
    //{
    //    public string objectName;
    //    public bool localPos;
    //    public Vector3 position;
    //    public bool useRotation;
    //    public Vector3 rotation;
    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ObjectToPosition : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        //public OTuple oTuple;

        public string objectName;
        public bool localPos;
        public Vector3 position;
        public bool useRotation;
        public Vector3 rotation;

        // This could probably be turned into a vector3 or transform, just uses GameObject.find which isn't 
        // great, but implementation is easier, I'll change this eventually
        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("ObjectToPosition", this);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("GameObject's Name: ", GUILayout.Width(135));
                objectName = EditorGUILayout.TextField(objectName);

            }
            localPos = EditorFields.Option(localPos, "Local Pos: ");
            useRotation = EditorFields.Option(useRotation, "Use Rotation: ");
            position = EditorFields.Vector3Field("Position: ", position);
            rotation = EditorFields.Vector3Field("Rotation: ", rotation);
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Write the name of a game object in the scene to " +
                "use it's transform for the position to teleport the object.", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif


    }
}

