//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
	[Serializable]
	public class SpriteFlash : IDisplayableDialogueEvent
	{
		public bool _showHelp = false;

		public void TriggerDialogueEvent()
		{
			DialogueEventSystem.TriggerEvent("SpriteFlash");
		}

		#if UNITY_EDITOR
		public void EditorUI(object value = null)
		{
			EditorGUILayout.HelpBox("The current character's sprite will flash at this point.", MessageType.Info, true);
		}
		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("Make the character's sprite flash.", MessageType.Info, true);
		}
		#endif
	}
}
