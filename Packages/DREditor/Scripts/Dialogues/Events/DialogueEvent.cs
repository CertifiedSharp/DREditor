//Dialogue Event script Author: SeleniumSoul for DREditor.
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine.Events;

namespace DREditor.Dialogues.Events
{
    public interface IDialogueEvent
    {
		void TriggerDialogueEvent();

		
	}

	public interface IDisplayableDialogueEvent : IDialogueEvent
	{
#if UNITY_EDITOR
        void EditorUI(object value = null);
        void ToggleHelpBox();
        void ShowHelpBox();
#endif
    }

    public class DiaEvent : UnityEvent<object>{ }

    static public class DialogueEventSystem
    {
        static readonly Dictionary<string, DiaEvent> _Events = new();


		public static void StartListening(List<string> eventNames, UnityAction<object> listener)
		{
			foreach (string eventName in eventNames) 
				StartListening(eventName, listener);
		}


		public static void StartListening(string eventName, UnityAction<object> listener)
		{
			if (_Events.TryGetValue(eventName, out DiaEvent thisEvent))
			{
				thisEvent.AddListener(listener);
			}
			else
			{
				thisEvent = new DiaEvent();
				thisEvent.AddListener(listener);
                _Events.Add(eventName, thisEvent);
			}
		}

        public static void StopListening(List<string> eventNames, UnityAction<object> listener)
        {
            foreach (string eventName in eventNames)
                StopListening(eventName, listener);
        }

        public static void StopListening(string eventName, UnityAction<object> listener)
		{
			if (_Events.TryGetValue(eventName, out DiaEvent thisEvent))
			{
				thisEvent.RemoveListener(listener);
			}
		}

		public static void TriggerEvents(List<string> eventList)
		{
			foreach(string dialogueEvent in eventList)
				TriggerEvent(dialogueEvent);
		}

		public static void TriggerEvent(string eventName)
		{
			if (_Events.TryGetValue(eventName, out DiaEvent thisEvent))
			{
				UnityEngine.Debug.Log("DREditor <color=blue>(DialogueEventSystem)</color>: Triggering " + eventName + " event.");
				thisEvent.Invoke(null);
			}
			else
			{
				DebugEventDoesntExist(eventName);
			}
		}

		public static bool IsEventRegistered(string eventName) => _Events.TryGetValue(eventName, out DiaEvent _);

        public static void TriggerEvent(string eventName, object value = null, bool verbose = true)
		{
			if (_Events.TryGetValue(eventName, out DiaEvent thisEvent))
			{
				if (verbose)
                    UnityEngine.Debug.Log($"DREditor <color=blue>(DialogueEventSystem)</color>: Triggering {eventName} event.");

                thisEvent.Invoke(value);
			}
			else
			{
				DebugEventDoesntExist(eventName);
			}
		}

        public static void TriggerEventIfListening(string eventName, object value = null, bool verbose = true)
        {
            if (IsEventRegistered(eventName))
                TriggerEvent(eventName, value, verbose);
        }

        public static void DebugEventDoesntExist(string eventName)
		{
            UnityEngine.Debug.LogWarning($"DREditor (DialogueEventSystem): Tryna trigger {eventName}, but it somehow doesn't exist." +
				$"\nYou need to have to use StartListening first before you invoke the event. This is ignorable if it's intended.");
        }
	}
}