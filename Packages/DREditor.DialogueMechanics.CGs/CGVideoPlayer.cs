//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CGHandler) + ", " + nameof(CGHandlerBase) + ", " + nameof(ICGHandler) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class CGVideoPlayer : CGHandler, ICGHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CGVideoProjector projector = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent stopAllAudio = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canToggleAutoMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent lastActor = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float startVideoFadeToDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startVideoFadeOutDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endVideoFadeToDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float endVideoFadeOutDuration = 1;


        /// <summary>
        ///
        /// </summary>
        protected VideoDisplay data;


        /// <inheritdoc/>
        protected override IEnumerator PlayCGEvent(object o, bool endState)
        {
            o ??= new VideoDisplay();

            data = (VideoDisplay)o;

            this.endState = endState;
            
            InitializeVideoEvent();

            yield return VideoEventProcess();

            ResolveCGEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeVideoEvent()
        {
            if (data.stopSound)
                stopAllAudio.Raise();

            if ((data.waitToEnd || data.playOnly) && !inTrialMode.Value)
                canToggleAutoMode.SetValue(false);

            if (!endState && inCGStateMode.Value && data.waitToEnd)
            {
                SetNameplatesTransparent();

                cgDialogueUIVisible.SetValue(false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator VideoEventProcess()
        {
            yield return PreparingStartVideoRoutine();

            if (data.ScreenFadeOut)
                yield return StartFadeIn();

            yield return EvaluateEventState();

            yield return FadeOutOnDark();

            if (data.waitToEnd)
                yield return WaitForVideoToEnd();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator PreparingStartVideoRoutine()
        {
            if (endState || inCGStateMode.Value)
                yield break;

            DialogueEventSystem.TriggerEvent(hideMainUIEventString);

            yield return new WaitForSeconds(hideNonCGUIWaitTime);

            if (!OtherHandlersActive() || data.ScreenFadeOut)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(startVideoFadeToDuration);

            TurnOffOtherHandlers();

            PrepareVideoUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareVideoUI()
        {
            cgUIVisible.SetValue(true);

            if (data.waitToEnd)
                cgDialogueUIVisible.SetValue(false);
            else
                ServiceLocator.GetService<SFXManagerBase>().PlaySFX(data.eventName);

            SetNameplatesTransparent();

            if (!data.playOnly && !data.waitToEnd)
                cgDialogueUIVisible.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator StartFadeIn()
        {
            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(startVideoFadeToDuration);

            if (data.eventName != null && !data.waitToEnd)
                ServiceLocator.GetService<SFXManagerBase>().PlaySFX(data.eventName);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EvaluateEventState()
        {
            if (!endState)
                yield return EvaluateActiveState();
            else if (inCGStateMode.Value)
                yield return EndVideoRoutine();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EvaluateActiveState()
        {
            if (data.iniClip)
                yield return projector.PlayStartingVideoTransition(data.iniClip, data.mainClip, false, data.speed, StartFadeOut());
            else if (!inCGStateMode.Value)
                yield return projector.PlayStartingVideoTransition(data.iniClip, data.mainClip, data.isLooping, data.speed, StartFadeOut(),
                    OtherHandlersActive());
            else
                yield return VideoToVideoRoutine();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator VideoToVideoRoutine()
        {
            if (data.eventName != null && data.waitToEnd)
                ServiceLocator.GetService<SFXManagerBase>().PlaySFX(data.eventName);

            yield return projector.PlayWhileInVideo(data.mainClip, data.isLooping, data.speed);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EndVideoRoutine()
        {
            if (!isGoingToMainMenu.Value)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(endVideoFadeToDuration);

            cgUIVisible.SetValue(false);

            TurnOffOtherHandlers();

            lastActor.SetValue(null);

            TransitionOffState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual IEnumerator StartFadeOut()
        {
            if (data.eventName != null && data.waitToEnd)
                ServiceLocator.GetService<SFXManagerBase>().PlaySFX(data.eventName);

            if (!isLoadingFile.Value)
            {
                yield return new WaitUntil(() => projector.VideoVisible());

                if (ServiceLocator.GetService<GlobalFade>().IsDark)
                    yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(startVideoFadeOutDuration);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForVideoToEnd()
        {
            yield return projector.WaitForVideoToFinish();

            if (data.playOnly)
                yield return EndVideoRoutine();
            else if (!string.IsNullOrEmpty(ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentLine().Text))
                cgDialogueUIVisible.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FadeOutOnDark()
        {
            if (ServiceLocator.GetService<GlobalFade>().IsDark && !isGoingToMainMenu.Value)
                if (!inTrialMode.Value)
                    yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(endVideoFadeOutDuration);
                else if (!isLoadingFile.Value)
                    ServiceLocator.GetService<GlobalFade>().FadeOut(endVideoFadeOutDuration);
                else
                    ServiceLocator.GetService<SoundManagerBase>().MuteAudio();
        }

        /// <inheritdoc/>
        protected override void ResolveCGEvent()
        {
            if (data.waitToEnd && !inTrialMode.Value)
                canToggleAutoMode.SetValue(true);

            base.ResolveCGEvent();
        }


        /// <inheritdoc/>
        public virtual void TransitionOffState()
        {
            projector.DisableVid();

            inCGStateMode.SetValue(false);
        }

        /// <inheritdoc/>
        public virtual BoolWithEvent InCGStateMode() => inCGStateMode;


        /// <inheritdoc/>
        public override void ToTitleScreen()
        {
            base.ToTitleScreen();

            TransitionOffState();
        }
    }
}