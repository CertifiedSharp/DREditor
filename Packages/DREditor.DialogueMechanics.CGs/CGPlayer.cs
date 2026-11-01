//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CGHandler) + ", " + nameof(CGHandlerBase) + ", " + nameof(ICGHandler) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class CGPlayer : CGHandler, ICGHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CGDisplayHandler cgDisplayHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float CGStartFadeToTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float CGStartFadeOutTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float CGEndFadeToTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float CGEndFadeOutTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float trialBufferFadeOutTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]

        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float flashbackStartFadeToTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float flashbackStartFadeOutTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float flashbackEndFadeToTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float flashbackEndFadeOutTime = 0.5f;


        [Header("")]
        [SerializeField] protected UnityEvent onFlashbackStart;

        [Header("")]
        [SerializeField] protected UnityEvent onFlashbackEnd;


        /// <summary>
        ///
        /// </summary>
        protected CGDisplay data;


        /// <inheritdoc/>
        protected override IEnumerator PlayCGEvent(object o, bool endState)
        {
            o ??= new CGDisplay();

            data = (CGDisplay)o;

            this.endState = endState;

            yield return CGEventProcess();

            ResolveCGEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator CGEventProcess()
        {
            yield return PreparingStartCGRoutine();

            if (data.ScreenFadeOut)
                yield return StartFadeIn();

            // Added 4-13-2026: To ensure prefab CG's don't have the text box over them during play only (intro title cards)
            // This line can be removed if there are display issues with the CG UI.
            // However if this line is removed then all prefab CG's canvas order must be 4 so that the text box does not block it.
            // Added 6-18-2026: !(endState && !inCGStateMode.Value) because skipping free time would display the cg dialogue UI.
            if (!(endState && !inCGStateMode.Value) && !ServiceLocator.GetService<UIHandlerBase>().MovingToTitleScreen)
                cgDialogueUIVisible.SetValue(SetUIVisibleState());

            yield return EvaluateEventState();

            if (InMiddleOfCG())
                TurnOffOtherHandlers();

            yield return FadeOutOnDark();

            yield return cgDisplayHandler.WaitForAnimatorToFinish(data.playOnly);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SetUIVisibleState()
        {
            bool notPlayOnlyPrefab = !(data.prefab != null && data.playOnly);

            if (isLoadingFile.Value)
                return notPlayOnlyPrefab;

            bool empty = string.IsNullOrEmpty(ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentLine().Text);

            return !empty && notPlayOnlyPrefab;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator PreparingStartCGRoutine()
        {
            if (endState || inCGStateMode.Value)
                yield break;

            DialogueEventSystem.TriggerEvent(hideMainUIEventString);

            yield return new WaitForSeconds(hideNonCGUIWaitTime);

            yield return CheckFadeToFlashback();

            bool otherHandlersActive = OtherHandlersActive();

            bool notFlashback = !(data.prefab == null && data.playOnly);

            bool nonFlashbackCGOnly = notFlashback && !otherHandlersActive;

            bool fullFadeDuringOtherState = otherHandlersActive && data.ScreenFadeOut;

            if (nonFlashbackCGOnly || fullFadeDuringOtherState)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(CGStartFadeToTime);

            if (otherHandlersActive)
                TurnOffOtherHandlers();

            PrepareCGUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator CheckFadeToFlashback()
        {
            if (!endState && data.prefab == null && data.playOnly && !ServiceLocator.GetService<GlobalFade>().IsDark)
            {
                onFlashbackStart?.Invoke();

                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(flashbackStartFadeToTime, true);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareCGUI()
        {
            cgUIVisible.SetValue(true);

            SetNameplatesTransparent();

            cgDialogueUIVisible.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator StartFadeIn()
        {
            yield return CheckFadeToFlashback();

            if (!ServiceLocator.GetService<GlobalFade>().IsDark && !(data.prefab == null && data.playOnly))
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(CGStartFadeToTime);

            ServiceLocator.GetService<SFXManagerBase>().PlaySFX(data.eventName); // Play Audio in-between scene change

            cgDisplayHandler.EndCGPrefab();

            SetNameplatesTransparent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EvaluateEventState()
        {
            if (StartingCG())
                yield return cgDisplayHandler.StartCGRoutine(data.CG, data.prefab);
            else if (InMiddleOfCG())
                yield return cgDisplayHandler.InCGRoutine(data.CG, data.prefab, data.transitionInstant, CheckFadeToFlashback());
            else
                yield return EndCGRoutine(data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool StartingCG() => !endState && !inCGStateMode.Value;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InMiddleOfCG() => !endState && inCGStateMode.Value;

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EndCGRoutine(CGDisplay data)
        {
            if (data.prefab == null && data.playOnly)// Flashback Variation
            {
                onFlashbackEnd?.Invoke();

                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(flashbackEndFadeToTime, true);
            }
            else
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(CGEndFadeToTime);

            cgUIVisible.SetValue(false);

            TransitionOffState();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FadeOutOnDark()
        {
            if (ServiceLocator.GetService<GlobalFade>().IsDark && !isLoadingFile.Value && !isGoingToMainMenu.Value)
            {
                if (data.prefab == null && data.playOnly)
                    yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(flashbackEndFadeOutTime, true);
                else if (!inTrialMode.Value)
                    yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(CGEndFadeOutTime);
                else
                    StartCoroutine(BufferFadeOut());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferFadeOut()
        {
            yield return new WaitForSeconds(trialBufferFadeOutTime);

            ServiceLocator.GetService<GlobalFade>().FadeOut(CGEndFadeOutTime);
        }


        /// <inheritdoc/>
        public virtual void TransitionOffState()
        {
            cgDisplayHandler.EndCGPrefab();

            inCGStateMode.SetValue(false);
        }

        /// <inheritdoc/>
        public virtual BoolWithEvent InCGStateMode() => inCGStateMode;
    }
}