//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace DREditor.Dialogues.Events
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ToggleBlur : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        public bool to;
        // This could probably be turned into a vector3 or transform, just uses GameObject.find which isn't 
        // great, but implementation is easier, I'll change this eventually
        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("ToggleBlur", to);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                to = EditorFields.Option(to, "Toggle to:", 90);
            }
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Toggle Blurring by checking the box", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif


    }
}
