//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.

namespace DREditor.Dialogues.Events
{
    using System;
    using UnityEditor;

    [Serializable]
    public class AnimationEvent : EventWaiter, IDisplayableDialogueEvent
    {
        //public AETuple a;
        public string EventName;


        public AnimationEvent() { }

        public AnimationEvent(string EventName) => this.EventName = EventName;

        public void TriggerDialogueEvent()
        {
            try
            {
                DialogueEventSystem.TriggerEvent(EventName, this);
            }
            catch
            {
                done = true;
                DialogueEventSystem.DebugEventDoesntExist(EventName);
            }
        }

#if UNITY_EDITOR
        public void EditorUI(object value = null)
        {
            EventName = EditorGUILayout.DelayedTextField("Invoke what Event?", EventName);
            EditorFields.Option(done, "View Done");
        }

        public void ShowHelpBox()
        {

        }

        public void ToggleHelpBox()
        {

        }


#endif
    }

}