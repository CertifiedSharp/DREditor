//Show CG Dialogue Event script Author: SeleniumSoul for DREditor.
//Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using UnityEngine.Video;
#if FMOD_UNITY
using FMODUnity;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
 //   [Serializable]
	//public struct SVTuple
 //   {
	//	public VideoClip iniClip;
	//	public VideoClip mainClip;
	//	public bool isLooping;
	//	public float speed;
	//	public bool ScreenFadeOut;
	//	public bool waitToEnd;
	//	public bool playOnly;
	//	public bool stopSound;
	//	[EventRef] public string eventName;
	//}

	/// <summary>
	///
	/// </summary>
	[Serializable]
	public class VideoDisplay : IDisplayableDialogueEvent
	{
		//public SVTuple SVValue;
		
		public VideoDisplay()
        {
			speed = 1;
        }
		public void TriggerDialogueEvent()
		{
			if (mainClip != null)
				DialogueEventSystem.TriggerEvent("ShowVideo", this);
			else
				DialogueEventSystem.TriggerEvent("HideVideo", this);
		}

        public VideoClip iniClip;
        public VideoClip mainClip;
        public bool isLooping;
        public float speed;
        public bool ScreenFadeOut;
        public bool waitToEnd;
        public bool playOnly;
        public bool stopSound;
#if FMOD_UNITY
        [EventRef] public string eventName;
#else
		public AudioClip eventName;
#endif 

#if UNITY_EDITOR
        private bool _showHelp = false;
		public void EditorUI(object value = null)
		{
			SerializedProperty c = (SerializedProperty)value;
			//SerializedProperty s = (SerializedProperty)value;
			c = c.FindPropertyRelative("eventName");
			
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("Main Clip: ", GUILayout.Width(70));
				mainClip = EditorFields.UnityField(mainClip, 160, 20);
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("Initial Clip: ", GUILayout.Width(70));
				iniClip = EditorFields.UnityField(iniClip, 160, 20);
			}
			isLooping = EditorFields.Option(isLooping, "Is Looping:", 70);
			if (isLooping && waitToEnd)
            {
				waitToEnd = false;
            }
			speed = EditorFields.FloatField("Playback Speed: ", speed, 30, 7);
			ScreenFadeOut = EditorFields.Option(ScreenFadeOut, "Use Full Fade:", 90);
			waitToEnd = EditorFields.Option(waitToEnd, "Wait to End:", 90);
			playOnly = EditorFields.Option(playOnly, "Play Only:", 90);
			stopSound = EditorFields.Option(stopSound, "Stop Sound:", 90);
			if (isLooping && waitToEnd)
			{
				isLooping = false;
			}
			EditorGUILayout.PropertyField(c, new GUIContent(""), GUILayout.MaxWidth(270));
		}

		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("Shows Videos. for animated CG's \n\nNote: You'll need the CG Player.", MessageType.Info, true);
		}
#endif
	}
}
