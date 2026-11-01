//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Loading;
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using DREditor.Services;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PreparableDialogueStarter) + ", " + nameof(IDialogueStartEvent) + ", " +
        nameof(PreparableDialogueStarter) + ", " + nameof(DialogueStarter) + ", " + nameof(IDialoguePrepareEvent) +
        "\n\n")]
    public class DialogueAssetStarter : PreparableDialogueStarter, IDialogueStartEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLeaving = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent saveDialogueName = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected DialogueWaiter DialogueWaiter = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueStarter InstantDialogueStarter = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string setDialogueControlKey = "dialogue";


        /// <summary>
        ///
        /// </summary>
        public event Action<DialogueCore> OnDialogueStarted;


        #region Start Dialogue

        /// <inheritdoc/>
        public override void StartDialogue<T>(SelectableObject actor, DialogueBase<T>[] dialogues, bool inleaving = false)
            => StartDialogueProcess(actor, dialogues, inleaving);

        public override void StartDialogue<T>(SelectableObject actor, DialogueBase<T> dialogue, bool inleaving = false) 
            => StartDialogue(actor, new DialogueBase<T>[1] { dialogue }, inleaving);

        /// <summary>
        ///
        /// </summary>
        /// <param name="coreActor"></param>
        /// <param name="coreDialogues"></param>
        /// <param name="inleaving"></param>
        protected virtual void StartDialogueProcess<T>(SelectableObject coreActor, DialogueBase<T>[] coreDialogues,
            bool inleaving = false) 
            where T : Line
        {
            if (SentDialoguesEmpty(coreDialogues))
                return;

            UnityEngine.Debug.Log($"{ToString()}: Starting Dialogue is named: " + coreDialogues[0].name);

            saveDialogueName.SetValue(coreDialogues[0].name);

            SetValueWithEvents(inleaving);

            if (coreDialogues[0] is IInstantAbleDialogue instantable && instantable.GetInstant())
                EvaluateInstantDialogue(coreActor, coreDialogues, inleaving);
            else
                StartCoroutine(BeginDialoguePreparations(coreActor, coreDialogues, inleaving));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="intendingToLeave"></param>
        protected virtual void SetValueWithEvents(bool intendingToLeave = false)
        {
            currentControlsKey.SetValue(setDialogueControlKey);

            hasMenuAccess.SetValue(false);

            isLeaving.SetValue(intendingToLeave);

            // Added 4-30-2026 Moving to a place that starts dialogue on load showed the reticle when it shouldn't, this fixes that.
            // The reason the other call in this script hasn't been removed is because it's unclear as to whether there would 
            // be any adverse affects from doing so, I mean without the call here, the issue mentioned wouldn't be happening.
            // If there are any adverse effects of inDialogue being active too early, then the issue mentioned must also be 
            // fixed if this line is to be removed.
            inDialogue.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogues"></param>
        /// <returns></returns>
        protected virtual bool SentDialoguesEmpty(DialogueCore[] dialogues)
        {
            bool dialoguesAreEmpty = dialogues == null || dialogues.Length == 0 || dialogues[0] == null;

            if (dialoguesAreEmpty)
            {
                ServiceLocator.GetService<IDialogueManager>().GetEnder().StopDialogue();

                UnityEngine.Debug.LogWarning($"{ToString()}:Sent dialogues were empty.");
            }

            return dialoguesAreEmpty;
        }

        #endregion

        #region Dialogue Preparations

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="dialogues"></param>
        /// <param name="inleaving"></param>
        protected virtual IEnumerator BeginDialoguePreparations<T>(SelectableObject actor, DialogueBase<T>[] dialogues,
            bool inleaving = false)
            where T : Line
        {
            InvokeDialoguePreparationEvent();

            yield return DialogueWaiter.CheckIfInDialogueToPlay(dialogues[0]);

            VerifyStartActor(actor);

            inDialogue.SetValue(true);

            OnDialogueStarted?.Invoke(dialogues[0]);

            UnityEngine.Debug.Log($"REPORT: {nameof(DialogueAssetStarter)}: " +
                $"Starting to Prepare Dialogue.");

            yield return PrepareDialogueComponents();

            UnityEngine.Debug.Log($"REPORT: {nameof(DialogueAssetStarter)}: " +
                $"Prepared Dialogue while Loading File is: " + isLoadingFile.Value);

            if (isLoadingFile.Value)
                dialogues = ServiceLocator.GetService<IDialogueManager>()
                .GetManagerComponent<DialogueLoader>()
                .StartLoadThrough(dialogues);

            ServiceLocator.GetService<IDialogueManager>()
                .GetReader()
                .PlayDialogues(dialogues, inleaving);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        protected virtual void VerifyStartActor(SelectableObject actor)
        {
            if (ServiceLocator.HasService<DialogueObjectManager>())
                ServiceLocator.GetService<DialogueObjectManager>().AttemptSetStartingDialogueObject(actor);
            else
                UnityEngine.Debug.LogWarning($"ALERT: There are no {nameof(DialogueObjectManager)}s in the scene," +
                    $" the starting actor will skip validity checking.");
        }

        #endregion

        #region Instant Dialogue

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="dialogues"></param>
        /// <param name="inleaving"></param>
        protected virtual void EvaluateInstantDialogue<T>(SelectableObject actor, DialogueBase<T>[] dialogues,
            bool inleaving = false) 
            where T : Line
        {
            if (InstantDialogueStarter == this)
            {
                UnityEngine.Debug.LogError("ALERT: ERROR: YOU CANNOT USE THE SAME DIALOGUE STARTER FOR INSTANT DIALOGUE, " +
                    "THIS WILL LOOP CODE. " +
                    "USE INSTANTDIALOGUESTARTER SCRIPT INSTEAD. REMOVING REFERENCE.");

                InstantDialogueStarter = null;
            }

            if (InstantDialogueStarter == null)
            {
                UnityEngine.Debug.LogWarning("REPORT: An Instant Dialogue Starter hasn't been assigned. Using Default Starter Instead.");

                StartCoroutine(BeginDialoguePreparations(actor, dialogues, inleaving));

                return;
            }

            InstantDialogueStarter.StartDialogue(actor, dialogues, inleaving);
        }

        #endregion
    }
}