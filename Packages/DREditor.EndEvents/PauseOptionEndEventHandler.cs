//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.UI.Menus;
using System;
using UnityEngine;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) +
        "\n\n")]
    public class PauseOptionEndEventHandler : DialogueComponent, IDialogueEndEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent pauseAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PauseOptionData pauseData = null;


        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
        {
            if (currentDialogue is Dialogue dialogue && (dialogue.UnlockPause || dialogue.UnlockPauseOption))
            {
                if (dialogue.UnlockPause)
                    pauseAccess.SetValue(true);

                if (dialogue.UnlockPauseOption)
                    pauseData.pauseOptions[dialogue.PauseOptionNum] = true;
            }

            return false;
        }
    }
}