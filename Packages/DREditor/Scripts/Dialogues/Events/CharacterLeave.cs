//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
	//[Serializable]
	//public struct CLTuple
	//{
	//	public bool appear;
	//	public bool current;
	//	public bool exit;
	//}
	[Serializable]
	public class CharacterLeave : IDisplayableDialogueEvent
	{
		
		//public CLTuple data;

        public bool appear;
        public bool current;
        public bool exit;

        public void TriggerDialogueEvent()
		{
			DialogueEventSystem.TriggerEvent("CharacterLeave", this);
		}

		#if UNITY_EDITOR
		private bool _showHelp = false;
		public void EditorUI(object value = null)
		{
			appear = EditorFields.Option(appear, "Appear: ");
			current = EditorFields.Option(current, "Current: ");
			exit = EditorFields.Option(exit, "Exit: ");
			//EditorGUILayout.HelpBox("Character sprite will fade out at this point.", MessageType.Info, true);
		}

		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("Appear is for if the character is appearing. " +
                "Current is to fade the current person speaking on the line (this should be false " +
                "if the protag is the one speaking on the line)." +
                "\n Exit Sends the actor to coordinates 0,-10 to hide the character.", MessageType.Info, true);
		}
		#endif
	}
}
