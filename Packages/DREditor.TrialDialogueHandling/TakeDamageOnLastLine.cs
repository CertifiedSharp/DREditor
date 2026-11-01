//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving.Handlers;
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Minigames.Stats;
using DREditor.Services;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineTextDisplay) +
        "\n\n")]
    public class TakeDamageOnLastLine : DialogueComponent, ILineTextDisplay
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent damage = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialMistake = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialContinued = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected HealthDataHandler<int> healthDataHandler = null;


        [Header("")]
        [SerializeField] protected UnityEvent OnDamageTaken;


        /// <inheritdoc/>
        public virtual void LineTextDisplay(Line currentLine)
        {
            var currentDialogue = ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentDialogue();

            if (currentLine == currentDialogue.GetLines<LineBase>()[^1] && DamageCanBeTaken())
                TakeDamageOnFinalLine();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DamageCanBeTaken() => !trialContinued.Value && 
            trialMistake.Value && ServiceLocator.HasService<TrialStatsBase>();

        /// <summary>
        ///
        /// </summary>
        protected virtual void TakeDamageOnFinalLine()
        {
            OnDamageTaken?.Invoke();

            healthDataHandler.TakeDamage(damage.Value);

            ServiceLocator.GetService<TrialStatsBase>().TakeDamage();
        }
    }
}