//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PreparableDialogueStarter) +
        "\n\n")]
    public class InstantDialogueStarter : PreparableDialogueStarter
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasStartedDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant = null;


        /// <inheritdoc/>
        public override void StartDialogue<T>(SelectableObject actor, DialogueBase<T>[] dialogues, bool inleaving = false) 
            => StartCoroutine(InstantDialogue(dialogues, inleaving));

        public override void StartDialogue<T>(SelectableObject actor, DialogueBase<T> dialogue, bool inleaving = false) 
            => StartCoroutine(InstantDialogue(new DialogueBase<T>[1] { dialogue }, inleaving));

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogues"></param>
        /// <param name="inleaving"></param>
        protected virtual IEnumerator InstantDialogue<T2>(DialogueBase<T2>[] dialogues, bool inleaving = false) 
            where T2 : Line
        {
            SetValueWithEvents();

            InvokeDialoguePreparationEvent();

            UnityEngine.Debug.Log($"REPORT: {nameof(InstantDialogueStarter)}: " +
                $"Starting to Prepare Dialogue.");

            yield return PrepareDialogueComponents();

            UnityEngine.Debug.Log($"REPORT: {nameof(InstantDialogueStarter)}: " +
                $"Finished preparing dialogue.");

            ServiceLocator.GetService<IDialogueManager>()
                .GetReader()
                .PlayDialogues(dialogues, inleaving);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetValueWithEvents()
        {
            inDialogue.SetValue(true);

            hasStartedDialogue.SetValue(true);

            inInstant.SetValue(true);
        }
    }
}