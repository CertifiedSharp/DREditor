//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.Services;
using System;
using UnityEngine;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) + 
        "\n\n")]
    public class MainMenuTransitionEndEventHandler : DialogueComponent, IDialogueEndEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent cleanUpDialogueReader = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ReturnToMenuHandler ReturnToMenuHandler = null;

        
        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> dialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
        {
            if (dialogue is Dialogue currentDialogue &&
                currentDialogue.SceneTransition.Enabled &&
                currentDialogue.SceneTransition.ToMenu)
            {
                cleanUpDialogueReader.Raise();

                ReturnToMenuHandler.ReturnToMenu(currentDialogue.SceneTransition.Scene);

                return true;
            }

            return false;
        }
    }
}