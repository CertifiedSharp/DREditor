//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct SOTuple
    //{
    //    public SOEnum type;
    //    public float duration;
    //    public float strength;
    //    public int vibrato;
    //    public float randomness;
    //    public bool snapping;
    //    public bool fadeOut;
    //    public bool protag;
    //}
    public enum SOEnum
    {
        Camera, CG
    }
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ShakeObject : IDisplayableDialogueEvent
    {
        
        //public SOTuple data;

        public SOEnum type;
        public float duration;
        public float strength;
        public int vibrato;
        public float randomness;
        public bool snapping;
        public bool fadeOut;
        public bool protag;

        public ShakeObject()
        {
            duration = 0.5f;
            strength = 4;
            vibrato = 10;
            randomness = 5;
            snapping = false;
            fadeOut = true;
        }

        public ShakeObject(SOEnum type, float duration, float strength, int vibrato, 
            float randomness, bool fadeOut)
        {
            this.type = type;
            this.duration = duration;
            this.strength = strength;
            this.vibrato = vibrato;
            this.randomness = randomness;
            this.fadeOut = fadeOut;
        }

        public void TriggerDialogueEvent()
        {
            switch (type)
            {
                case SOEnum.Camera:
                    DialogueEventSystem.TriggerEvent("ShakeCamera", this);
                    break;
                case SOEnum.CG:
                    DialogueEventSystem.TriggerEvent("ShakeCG", this);
                    break;
            }
        }

#if UNITY_EDITOR
        public bool _showHelp = false;
        public void EditorUI(object value = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                type = (SOEnum)EditorGUILayout.EnumPopup(type);

                if (GUILayout.Button("Test", GUILayout.Width(70)))
                {
                    if (!Application.isPlaying)
                    {
                        EditorUtility.DisplayDialog("Must be in Play Mode", "You must be in play mode to test!", "Ok");
                        return;
                    }

                    TriggerDialogueEvent();
                }
            }
            
            duration = FloatField("Duration: ", duration);
            strength = FloatField("Strength: ", strength);
            vibrato = IntField("Vibrato: ", vibrato);
            randomness = FloatField("Randomness: ", randomness);
            snapping = Option("Snapping: ", snapping);
            fadeOut = Option("Fade Out: ", fadeOut);
            protag = Option("Protag: ", protag);
        }
        public static int IntField(string name, int value, int textBoxWidth = 200, int labelBoxMulti = 6)
        {
            GUI.backgroundColor = Color.white;
            int result;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(name, GUILayout.Width(name.Length * labelBoxMulti));
                result = EditorGUILayout.IntField(value, GUILayout.Width(textBoxWidth));
            }
            GUILayout.FlexibleSpace();
            return result;
        }

        public static float FloatField(string label, float value, int textBoxWidth = 200, int labelBoxMulti = 6)//*Added int field functionality
        {
            GUI.backgroundColor = Color.white;
            float result;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(label.Length * labelBoxMulti));
                result = EditorGUILayout.FloatField(value, GUILayout.Width(textBoxWidth));
            }
            GUILayout.FlexibleSpace();
            return result;
        }
        public static bool Option(string label, bool option, float labelWidth = 60)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(labelWidth));
                return EditorGUILayout.Toggle(option, GUILayout.Width(15));
            }
            
        }
        public void ShowHelpBox()
        {
            if (_showHelp) EditorGUILayout.HelpBox("Shake either the Camera or CG/Video using DOTween aspects.", MessageType.Info, true);
        }

        public void ToggleHelpBox()
        {
            _showHelp = !_showHelp;
        }
#endif
    }
}
