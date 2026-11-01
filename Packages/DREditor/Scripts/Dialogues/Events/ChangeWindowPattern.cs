//Author: SeleniumSoul
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace DREditor.Dialogues.Events
{
	/// <summary>
	///
	/// </summary>
	[Serializable]
	public class ChangeWindowPattern : IDisplayableDialogueEvent
	{
		public bool _showHelp = false;
		//public CWPTuple CWPValue;

        public string PatternName;

        public int parameterIndex;

        public void SetParameterIndex(int parameterIndex) => this.parameterIndex = parameterIndex;

        public void TriggerDialogueEvent()
		{
			DialogueEventSystem.TriggerEvent("ChangeWindowPattern", this);
		}

	#if UNITY_EDITOR
		private AnimatorController PanelAnimator;
		private string[] parameters;

		public void EditorUI(object value = null)
		{
            EditorGUILayout.HelpBox("ALERT: This event is outdated and unused by the system and while currently obsolete, " +
				"was considered useful enough to keep within the engine." +
				"\nIf you want to use this event you must program a script that listens to it.", 
				MessageType.Warning, true);

            if (PanelAnimator == null)
			{
				var x = AssetDatabase.FindAssets("DialoguePanels t:AnimatorController");

				if (x.Length == 0)
					return;

                string _animPath = AssetDatabase.GUIDToAssetPath(x[0]);
				PanelAnimator = AssetDatabase.LoadAssetAtPath<AnimatorController>(_animPath);
			}
			PanelAnimator = (AnimatorController)EditorGUILayout.ObjectField("Animator: ", PanelAnimator, typeof(AnimatorController), true);
			if (PanelAnimator)
			{
				parameters = GetAnimatorParameters();
				SetParameterIndex(EditorGUILayout.Popup("Position: ", parameterIndex, parameters));
				PatternName = parameters[parameterIndex];
				EditorGUILayout.LabelField($"Showing {PatternName} at this line.");
			}
            else
            {
				PatternName = EditorGUILayout.DelayedTextField("Position: ", PatternName);
				EditorGUILayout.HelpBox("Please link the AnimatorController of your Dialogue Panel on the Animator Field. Or if unavailable, " +
					"specify the parameter in the field above.", MessageType.Warning, true);
			}
		}

		public void ToggleHelpBox()
		{
			_showHelp = !_showHelp;
		}

		public void ShowHelpBox()
		{
			if (_showHelp) EditorGUILayout.HelpBox("Change the positions of the panel windows when talking to somebody.", MessageType.Info, true);
		}

		private string[] GetAnimatorParameters()
		{
			string[] parameterlist = new string[PanelAnimator.parameters.Length - 2];

			for (int i = 0; i < PanelAnimator.parameters.Length - 2; i++)
            {
				parameterlist[i] = PanelAnimator.parameters[i + 2].name;
            }

			return parameterlist;
		}
	#endif
	}
}
