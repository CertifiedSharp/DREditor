//Original Author: Selenium Soul
//Completely Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(Dialogue))]
	public class DialogueEditor : DialogueEditorBase<Line>
    {
        /// <summary>
        ///
        /// </summary>
        protected Dialogue dialogue;


        /// <summary>
        ///
        /// </summary>
        protected readonly DialogueDebugToolEditor<Line> debugTool = new();

        /// <summary>
        ///
        /// </summary>
        protected readonly LineEditor<Line> lineEditor = new();

        /// <summary>
        ///
        /// </summary>
        protected DialogueEndEventEditor endEventEditor = new();
        

        protected override LineEditorBase LineEditor { get => lineEditor; }


        protected override DialogueDebugToolEditorBase DebugTool { get => debugTool; }


        public virtual void OnEnable()
		{
            SetDialogue(target as Dialogue);

            lineEditor.Initialize(dia, serializedObject, this);
		}


        protected override void SetDialogue(DialogueBase<Line> dia)
        {
            base.SetDialogue(dia);

            dialogue = this.dia as Dialogue;
        }


        protected override void DisplayDialogueOptions()
        {
            base.DisplayDialogueOptions();
            
            CustomLayoutUtility.MassHorizontal(UnlockPauseMenu, UnlockSpecificPauseOption, UpdateAllowedPauseOption);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnlockPauseMenu() => 
            dialogue.UnlockPause = HandyFields.Option(dialogue.UnlockPause, "Unlock Pause: ", 90);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnlockSpecificPauseOption()
        {
            dialogue.UnlockPauseOption = HandyFields.Option(dialogue.UnlockPauseOption, "Unlock Option: ", 95);

            if (dialogue.UnlockPauseOption)
                dialogue.PauseOptionNum = HandyFields.IntField("Option Number: ", dialogue.PauseOptionNum, 20, 7);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateAllowedPauseOption()
        {
            if (dialogue.UnlockPauseOption)
                dialogue.PauseOptionTo = HandyFields.Option(dialogue.PauseOptionTo, "To: ", 25);
        }


        protected override void DisplayFooter()
        {
            if (dialogue == null)
                UnityEngine.Debug.LogWarning("DIALOGUE NULL");

            if (endEventEditor == null)
                UnityEngine.Debug.LogWarning("EDITOR NULL");

            endEventEditor ??= new();

            endEventEditor.DisplayEndEvents(dialogue);

            base.DisplayFooter();

            endEventEditor.DisplayEndEventsButtons();

            GUILayout.Space(30);
		}
    }
}
#endif