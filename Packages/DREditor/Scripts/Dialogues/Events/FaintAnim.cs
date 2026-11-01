//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct FATuple
    //{
    //    public bool faint;
    //    public float to;
    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class FaintAnim : IDisplayableDialogueEvent
    {
        public bool _showHelp = false;
        //public FATuple data;

        public bool faint;
        public float to;

        public void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEvent("FaintAnim", this);
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                faint = EditorFields.Option(faint, "Faint: ");
            }
            to = EditorFields.FloatField("To Y: ", to);
        }
        public static T UnityField<T>(T data, int width = 120, int height = 120) where T : UnityEngine.Object
        {
            T result;
            using (new EditorGUILayout.VerticalScope())
            {
                result = (T)EditorGUILayout.ObjectField(data, typeof(T), false, GUILayout.Width(width), GUILayout.Height(height));
            }
            return result;
        }

        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("If faint is true then character will faint, " +
                "if faint is false then the character will get up.", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif
    }
}
