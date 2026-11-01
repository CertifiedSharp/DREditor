//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CGFlash : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("CGFlash");
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            EditorGUILayout.LabelField("CG will flash white", GUILayout.Width(135));
        }
        
        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Flashes the CG White", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif
    }
}
