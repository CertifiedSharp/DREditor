//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Minigames.Stats;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineTextDisplayRoutine) +
        "\n\n")]
    public class StatsGameOverContinue : DialogueComponent, ILineTextDisplayRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialContinued = null;


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine)
        {
            var currentDialogue = ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentDialogue();

            return currentLine == currentDialogue.GetLines<LineBase>()[^1] && trialContinued.Value;
        }

        /// <inheritdoc/>
        public virtual IEnumerator LineTextDisplay(Line currentLine)
        {
            yield return ServiceLocator.GetService<TrialStatsBase>().GameOverResetProcess();
        }
    }
}