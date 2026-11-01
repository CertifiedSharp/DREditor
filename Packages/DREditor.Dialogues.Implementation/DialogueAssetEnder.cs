//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEnder) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class DialogueAssetEnder : DialogueEnder, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isWaiting = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isGoingToMainMenu = null;

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

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected DialogueEnder InstantDialogueEnder = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueFinisher DialogueFinisher = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> EarlyResolvableComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int collectionID = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string orderFieldName = "resolveOrder";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueComponentCollection<IResolvableDialogueComponent> resolvableCollection = new();

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("ResolvableComponents")]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> overrideComponents = new();


        protected virtual void Awake() => resolvableCollection.Initialize(transform, orderFieldName, collectionID);

        protected virtual void OnValidate() => resolvableCollection.Initialize(transform, orderFieldName, collectionID);


        /// <inheritdoc/>
        public override void StopDialogue() => StartCoroutine(EndDialogue());

        /// <inheritdoc/>
        public override IEnumerator EndDialogue(bool useDialogueFinisher = true)
        {
            UnityEngine.Debug.Log($"REPORT: {nameof(DialogueAssetEnder)}: Ending Dialogue process started.");

            yield return ResolveDialogueComponentsEarly();


            bool stopEarly = false;

            yield return CheckStopProcessEarly(useDialogueFinisher, () => { stopEarly = true; });

            if (stopEarly)
                yield break;


            yield return ResolveDialogueComponents();

            ResolveDialogueEventHandler();

            if (useDialogueFinisher)
                FinishDialogue();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ResolveDialogueComponentsEarly()
        {
            foreach (var component in EarlyResolvableComponents)
                if (component != null && component is IEarlyResolvableDialogueComponent resolvable)
                    yield return resolvable.ResolveEarly();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="finishDialogue"></param>
        /// <param name="stopEarlyCallback"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual IEnumerator CheckStopProcessEarly(bool finishDialogue = true, Action stopEarlyCallback = null)
        {
            if (isWaiting.Value)
            {
                isWaiting.Value = false;

                stopEarlyCallback?.Invoke();

                yield break;
            }

            if (inInstant.Value)
            {
                if (InstantDialogueEnder != null && InstantDialogueEnder != this)
                {
                    yield return InstantDialogueEnder.EndDialogue(finishDialogue);

                    stopEarlyCallback?.Invoke();

                    yield break;
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"ALERT: {nameof(DialogueAssetEnder)}: The alternate Dialogue Ender" +
                        " was either null or recursively implemented. Using Default.");

                    inInstant.Value = false;
                }
            }

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveDialogueComponents()
        {
            IEnumerable componentList = overrideComponents.Count > 0 ? overrideComponents : 
                resolvableCollection.components.OrderBy(com => com.Order);

            foreach (var component in componentList)
                if (component != null && component is IResolvableDialogueComponent resolvable)
                    if (resolvable.UseRoutineToResolve())
                        yield return resolvable.ResolveRoutine();
                    else
                        resolvable.Resolve();

            yield break;
        }

        /// <inheritdoc/>
        public override void FinishDialogue() => DialogueFinisher.FinishDialogue();


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => ResetDialogueIfNotInTrial();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetDialogueIfNotInTrial()
        {
            if (inDialogue.Value && !inTrialMode.Value)
            {
                UnityEngine.Debug.Log($"REPORT: {nameof(DialogueAssetEnder)}: " +
                    $"Stopping dialogue to reset the system.");

                SetGoingToMainMenuModeOn();

                StopDialogue();
            }
            else
                UnityEngine.Debug.Log($"REPORT: {nameof(DialogueAssetEnder)}: " +
                    $"Gameplay wasn't in dialogue so not resetting when going to title screen");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetGoingToMainMenuModeOn() => isGoingToMainMenu.SetValue(true);
    }
}