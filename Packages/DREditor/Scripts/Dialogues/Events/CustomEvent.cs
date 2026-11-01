//Original Author: SeleniumSoul for DREditor.
//Completely Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
 //   [Serializable]
	//public struct CETuple
	//{
	//	public string EventName;
	//	public int ExecutionOrder;
 //   }

	[Serializable]
	public class CustomEvent : IDisplayableDialogueEvent
	{
		public bool _showHelp = false;

        //public CETuple CEValue;

        public string EventName;
        public int ExecutionOrder;

        [FormerlySerializedAs("_ToggleCustom")]
		public bool toggleCustom = false;

        [FormerlySerializedAs("_Index")]
        public int index = 0;

        public virtual void SetIndex(int index) => this.index = index;

		[SerializeReference] IDataCapsulate dataCap;

		
		
        public void TriggerDialogueEvent() => DialogueEventSystem.TriggerEvent(EventName, dataCap);

#if UNITY_EDITOR
        List<string> availableCustomEvents = new();

        CustomDialogueEventAttribute eventAttribute;

        public void ReplaceList(List<string> list) => this.availableCustomEvents = list;

		public void EditorUI(object value = null)
		{
			EventName = EditorGUILayout.DelayedTextField("Invoke what Event?", EventName);

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("Execution Order: ", GUILayout.Width(100));

                ExecutionOrder = EditorGUILayout.IntField(ExecutionOrder, GUILayout.Width(25));
            }

			using (new EditorGUILayout.HorizontalScope())
			{
				bool cache = toggleCustom;

				DisplayCustomToggle();

				if (cache != toggleCustom || toggleCustom && GUILayout.Button("Refresh", GUILayout.MaxWidth(200)))
				{
                    availableCustomEvents ??= new();

                    LookForAttribute<CustomDialogueEventAttribute>((attribute) =>
                    {
                        availableCustomEvents.Add(attribute.eventName);

                        return false;
                    });

                    RefreshPopupIndex();
                }
			}

			if (toggleCustom)
			{
                if (availableCustomEvents.Count == 0)
                    EditorGUILayout.LabelField("No Classes inherit the Custom Event Attribute " +
                        "or you need to press \"Refresh\"!");
                else if (index >= availableCustomEvents.Count)
                    RefreshPopupIndex();
                else
                    DisplayCustomEvent();
            }
			else
				OnNotUsingCustom();
        }

		protected virtual void DisplayCustomToggle()
		{
            GUILayout.Label("Use Custom: ", GUILayout.Width(80));

            toggleCustom = EditorGUILayout.Toggle(toggleCustom, GUILayout.Width(15));
        }

		protected virtual void RefreshPopupIndex()
		{
            // Even if this happens the actual data saved prior to this problem occuring should still be there, so you'd
            // just have to change the index , like index = list.IndexOf(EventName);
            if (availableCustomEvents.IndexOf(EventName) >= 0)
                index = availableCustomEvents.IndexOf(EventName);
            else
                EditorGUILayout.LabelField("ALERT: " +
                    "Internal index out of range, please go to => DREditor/Utility/Verify/Custom Events." +
                    "\nIf the Tag was removed and this was intentional, you can either delete this event or " +
                    "\ntoggle off the \"Use Custom\" Option",
					GUILayout.MinHeight(70));
        }

		protected virtual void DisplayCustomEvent()
		{
            DisplayEventNamePopup();

            if (GUILayout.Button("Spawn This Events Relevant Data"))
                SpawnReleventData();

            DisplayCustomEventData();
        }

		protected virtual void DisplayEventNamePopup()
		{
            index = EditorGUILayout.Popup(index, availableCustomEvents.ToArray());

            EventName = availableCustomEvents[index];
        }

		protected virtual void DisplayCustomEventData() =>
            dataCap?.Display(eventAttribute != null && eventAttribute.pass != null ? eventAttribute.pass : dataCap);

        protected virtual void SpawnReleventData()
		{
            UnityEngine.Debug.Log("AFFIRMATIVE: Creating Instance");

            LookForAttribute<CustomDialogueEventAttribute>((attribute) =>
            {
                bool result = attribute.eventName == EventName;

                eventAttribute = result ? attribute : null;

                return result;
            });

            try
            {
                if (dataCap == null && eventAttribute != null && eventAttribute.type != null)
                    dataCap = (IDataCapsulate)Activator.CreateInstance(eventAttribute.type);
            }
            catch (Exception exception)
            {
                OnSpawningFailed(exception);
            }
        }

		protected virtual void OnSpawningFailed(Exception exception)
		{
			UnityEngine.Debug.LogWarning("ALERT: Spawning the Relevant Data Instance has failed, see below for details.");

            if (eventAttribute.type.IsAssignableFrom(typeof(IDataCapsulate)))
                UnityEngine.Debug.LogException(exception);
			else
                UnityEngine.Debug.LogWarning("ANALYSIS: The Data could not be spawned because the class did not inherit " 
					+ nameof(IDataCapsulate) + 
					$"PROPOSAL: Use a class inheriting {nameof(IDataCapsulate)} " +
					$"when labeling a custom event attribute in code");
        }

		protected virtual void OnNotUsingCustom()
		{
            availableCustomEvents.Clear();

            dataCap = null;
        }
		
		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) 
				EditorGUILayout.HelpBox("Make the Dialogue Event System invoke a custom event using a string." +
				"\nDoes not accept custom parameters." +
				"\n\nCaution: This is intended to be used as a temporary debugging solution. " +
				"It is highly recommended to make a new Dialogue Event Script to avoid having the event being forgotten.",
				MessageType.Info, true);
		}


		static void LookForAttribute<T>(Func<T, bool> check) where T : Attribute
		{
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
				foreach (Type type in assembly.GetTypes())
					if (type.GetCustomAttributes(typeof(T)) is IEnumerable<T> attributes &&
						attributes.Count() > 0)
						foreach (T attribute in attributes)
							if (attribute is T c && check(c))
								return;
        }
#endif
    }
}
