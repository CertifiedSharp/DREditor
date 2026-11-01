//Show CG Dialogue Event script Author: SeleniumSoul for DREditor.
//Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Dialogues.Events
{
    //[Serializable]
    //public struct FBTuple
    //{
    //	public List<Sprite> Flashbacks;
    //}
    /// <summary>
    ///
    /// </summary>
    [Serializable]
	public class Flashback : IDisplayableDialogueEvent
	{
		//public FBTuple FBValue;
        public List<Sprite> Flashbacks;

        public void TriggerDialogueEvent()
		{
			DialogueEventSystem.TriggerEvent("Flashback", this);
		}

#if UNITY_EDITOR
		private bool _showHelp = false;
		public void EditorUI(object value = null)
		{
			if(Flashbacks == null)
            {
				Flashbacks = new List<Sprite>();
            }
			for(int i = 0; i < Flashbacks.Count; i++)
            {
				using (new EditorGUILayout.HorizontalScope())
                {
					Flashbacks[i] = EditorFields.SpriteField("FB: ", Flashbacks[i], 120, 30);
					if (GUILayout.Button("x", GUILayout.Width(20)))
					{
						Flashbacks.RemoveAt(i);
					}
				}
				
			}

			GUILayout.Space(25);

			if (GUILayout.Button("Add Flashback", GUILayout.Width(120)))
            {
				Flashbacks.Add(null);
            }
		}

		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("List of Flash Back CG's to input", MessageType.Info, true);
		}
#endif
	}
}
