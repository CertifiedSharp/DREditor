//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
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
    public class TrialTutorial : IDisplayableDialogueEvent
    {
        public ScriptableObject asset;
        public bool _showHelp = false;
        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("TrialTutorial", asset);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            asset = EditorFields.UnityField(asset, 120, 50);
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Starts Trial Tutorial, leave empty to turn off active tutorial", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif
    }
}
