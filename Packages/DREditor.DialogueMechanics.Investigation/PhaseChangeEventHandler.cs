//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Progressing.Handling;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhaseChangeListener<StartInvestigation>) + " for the type: " + nameof(StartInvestigation) +
        "\n\n")]
    public class PhaseChangeEventHandler : PhaseChangeListener<StartInvestigation>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasStartedDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string disableControlsEventString = "DisableControlScripts";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent PrePhaseChange = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent PrePhaseDialogueEnd = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent PostPhaseChange = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent moveToNextNaturalObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent onRoomInstanceLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string loadPhaseRoomEventString = "LoadInvestigationRoom";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent transitionPhaseUI = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool onlyForOneID = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int forPhaseID = -1;


        /// <summary>
        ///
        /// </summary>
        protected IUIVisibility roomLoadPhaseTransitionUI = null;


        /// <summary>
        ///
        /// </summary>
        protected override void Start()
        {
            base.Start();

            if (transitionPhaseUI != null)
                roomLoadPhaseTransitionUI = transitionPhaseUI as IUIVisibility;
        }

        
        /// <inheritdoc/>
        protected override void OnPhaseChange(StartInvestigation phaseData)
        {
            if (onlyForOneID)
                if (forPhaseID == phaseData.ID)
                    StartCoroutine(OnPhaseChangeRoutine(phaseData));
                else
                    return;
            else
                StartCoroutine(OnPhaseChangeRoutine(phaseData));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phaseData"></param>
        protected virtual IEnumerator OnPhaseChangeRoutine(StartInvestigation phaseData)
        {
            DialogueEventSystem.TriggerEvent(disableControlsEventString);

            if (phaseData.NewDialogue != null)
                hasStartedDialogue.SetValue(false);

            PrePhaseChange.Raise();

            yield return ServiceLocator.GetService<IDialogueManager>().GetEnder().EndDialogue(false);

            PrePhaseDialogueEnd.Raise();

            // Dialogue has Ended here, Starting Phase Change

            yield return roomLoadPhaseTransitionUI?.SetVisibilityRoutine(true);

            if (phaseData.NextObjective)
                yield return MoveToNextObjective();

            IPhaseStartHandler gameEvent = GetPhaseStartHandler(phaseData.ID);

            if (gameEvent == null)
                UnityEngine.Debug.Log($"REPORT: {ToString()}: No game phase start handler found, so skipping.");

            gameEvent?.AfterLoadingRoom();

            yield return roomLoadPhaseTransitionUI?.SetVisibilityRoutine(false);

            // Room has Loaded Next Objective Room if used and faded out of black screen
            // and now the Start Investigation animation must show

            yield return gameEvent?.OnPhaseStart(phaseData);

            // At this point the Phase Start! Animation has Finished and you can give the player control 
            // and show the main UI + Reticle

            PostPhaseChange.Raise();

            PlayDialogueOnPhaseChanged(phaseData.NewDialogue);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual IPhaseStartHandler GetPhaseStartHandler(int id) 
            => FindObjectsByType<MonoBehaviour>()
                .OfType<IPhaseStartHandler>()
                .Where(startHandler => startHandler.ID == id)
                .FirstOrDefault();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator MoveToNextObjective()
        {
            moveToNextNaturalObjective.Raise();

            yield return null;

            DialogueEventSystem.TriggerEvent(loadPhaseRoomEventString,
                ServiceLocator.GetService<ProgressionManagerBase>().currentObjective);

            yield return new WaitUntil(() => onRoomInstanceLoaded.Value);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        protected virtual void PlayDialogueOnPhaseChanged(Dialogue dialogue)
        {
            if (dialogue != null)
                ServiceLocator.GetService<IDialogueManager>().StartDialogue(null, dialogue);
            else
                ServiceLocator.GetService<IDialogueManager>().GetEnder().FinishDialogue();
        }
    }
}