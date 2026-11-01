//Change Character Focus Dialogue Event script Author: SeleniumSoul for DREditor.

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
	public enum FadeChoices { FadeIn, FadeOut }
	/// <summary>
	///
	/// </summary>
	[Serializable]
	public class ScreenTransitions : IDisplayableDialogueEvent
	{
		public bool _showHelp = false;
        public FadeChoices FadeChoice;
        public float FadeSpeed;

        public void TriggerDialogueEvent()
		{
			switch (FadeChoice)
			{
				case FadeChoices.FadeIn:
					DialogueEventSystem.TriggerEvent("FadeOut", FadeSpeed);
					break;
				case FadeChoices.FadeOut:
					DialogueEventSystem.TriggerEvent("FadeToBlack", FadeSpeed);
					break;
				default:
					UnityEngine.Debug.LogError("DREditor (ScreenTransitions): Unable to recognize transition! " +
						"Please check the transition option in the Dialogue Event if it is empty.");
					break;
			}
		}

#if UNITY_EDITOR
		public void EditorUI(object value = null)
		{
			FadeChoice = (FadeChoices)EditorGUILayout.EnumPopup(new GUIContent("Transition", "Choose which transition to do."), FadeChoice);
			FadeSpeed = EditorGUILayout.DelayedFloatField(new GUIContent("At what speed? (in seconds)", 
				"Time in seconds in which the transition will play."), FadeSpeed);
		}
		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("Make the whole screen fade to black.", MessageType.Info, true);
		}
#endif
	}
}
