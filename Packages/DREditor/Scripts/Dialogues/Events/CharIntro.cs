//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct CITuple
    //{
    //    public string charName;
    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class CharIntro : EventWaiter, IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        //public CITuple cITuple;

        public string charName;

        public CharIntro() { }

        public CharIntro(string charName) => this.charName = charName;

        // This could probably be turned into a vector3 or transform, just uses GameObject.find which isn't 
        // great, but implementation is easier, I'll change this eventually
        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("CharIntro", this);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Char's First Name: ", GUILayout.Width(135));
                charName = EditorGUILayout.TextField(charName);

            }
            
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Calls for the Character Intro Animation", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif


    }
}
