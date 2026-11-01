//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.RoomLoading;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TrialEnderBase) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class TrialEnder : TrialEnderBase, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent moveToNextNaturalObjective = null;

        [Tooltip("")]
        [SerializeField] protected SceneEvent triggerAllRequiredFlags = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent requiredFlagsTriggered = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentAddState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int addStateIndex = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent endTrialVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> resolveDialogueEvents = new() { "EnableMainCamera" };

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> exitDialogueEvents = new() { "ResolveTrial" };


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool instantFadeOut = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endSceneFadeToDuration = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endSceneFadeOutDuration = 0.3f;


        /// <summary>
        ///
        /// </summary>
        protected TrialBuilder trial;

        /// <summary>
        ///
        /// </summary>
        protected bool attemptedRestoration = false;


        protected virtual void OnDisable() => DialogueEventSystem.TriggerEvents(exitDialogueEvents);


        /// <inheritdoc/>
        protected override IEnumerator PreEndTrialProcess()
        {
            if (endTrialVisibility != null)
                yield return endTrialVisibility.ShowTillComplete();

            if (instantFadeOut)
                ServiceLocator.GetService<GlobalFade>().FadeTo(0);
            else
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(endSceneFadeToDuration);
        }

        /// <inheritdoc/>
        protected override void Initialize(ScriptableObject builder)
        {
            trial = (TrialBuilder)builder;

            currentAddState.SetValue(addStateIndex);
        }

        /// <inheritdoc/>
        protected override bool TrialEnderWillFail()
        {
            string alertReason = string.Empty;

            bool destinationMissing = !trial.EndGate && string.IsNullOrEmpty(trial.EndSceneName);

            if (destinationMissing)
                alertReason = "ALERT: The current trial builder does not specify a place to leave to. ";

            bool doubleDestinations = trial.EndGate && !string.IsNullOrEmpty(trial.EndSceneName);

            if (doubleDestinations)
                alertReason = "ALERT: The system cannot decide which scene to exit using, the gate or scene name?";

            if (!debug && (doubleDestinations || destinationMissing))
                UnityEngine.Debug.LogError(alertReason +
                    $"\nPROPOSAL: Fill out either the gate OR the scene name on the trial asset, do NOT fill out both." +
                    $"\nREPORT: The game will now stop.");

            return destinationMissing || doubleDestinations;
        }

        /// <inheritdoc/>
        protected override bool PlayerCanLeaveTrial()
        {
            bool result = requiredFlagsTriggered.Value;

            if (!result)
                UnityEngine.Debug.LogWarning("ALERT: The trial cannot be left because all required flags have not been triggered!" +
                    "\nHYPOTHESIS: It is likely that the unit this pod is assigned to is debugging gameplay or a save file has been corrupted.");

            return result;
        }

        /// <inheritdoc/>
        protected override void LeaveTrial()
        {
            UnityEngine.Debug.Log("REPORT: CLEARED INSTANCE DATA AND CHANGING OBJECTIVE");

            moveToNextNaturalObjective.Raise();

            if (trial.EndGate != null)
                ServiceLocator.GetService<RoomChangerBase>().SetDestination(trial.EndGate);

            ServiceLocator.GetService<InstanceManagerBase>().ClearRoomData();

            DialogueEventSystem.TriggerEvents(resolveDialogueEvents);

            DialogueEventSystem.TriggerEvents(exitDialogueEvents);

            onFinishedLoadingRoom.RegisterListener(this);

            UnityEngine.Debug.LogWarning("CHANGING SCENE");

            string destinationSceneName = string.IsNullOrEmpty(trial.EndSceneName) ? trial.EndGate.ToAreaName : trial.EndSceneName;

            SceneManager.LoadSceneAsync(destinationSceneName);
        }

        /// <inheritdoc/>
        protected override void CannotLeaveTrial()
        {
            if (!attemptedRestoration)
                AttemptLeaveTrialCorrection();
            else
                OnLeaveTrialCorrectionFailed();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptLeaveTrialCorrection()
        {
            UnityEngine.Debug.Log("REPORT: Attempting to restore the system by triggering all flags.");

            attemptedRestoration = true;

            triggerAllRequiredFlags.Raise();

            EvaluateLeaveTrial();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnLeaveTrialCorrectionFailed()
        {
            UnityEngine.Debug.LogError("ALERT: FAILED TO CORRECT ABILITY FOR PLAYER TO LEAVE THE TRIAL THE GAME WILL NOT CONTINUE.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        /// <inheritdoc/>
        public virtual void OnEventRaised() => OnEndSceneLoaded();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEndSceneLoaded()
        {
            onFinishedLoadingRoom.UnregisterListener(this);

            ServiceLocator.GetService<GlobalFade>().FadeOut(endSceneFadeOutDuration);
        }
    }
}