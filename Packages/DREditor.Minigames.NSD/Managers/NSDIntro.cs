//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Cameras;
using DREditor.Dialogues;
using DREditor.UIHandling;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace DREditor.Minigames.NSD.Managers
{
    //I commented the process so that it'd be easier for others to understand the sequence of events,
    //not that the code doesn't explain it well enough already.
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) +
        "\n\n")]
    public class NSDIntro : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string transitionCameraAnimationName = "NSD-transition";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image transitionBlackFade = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float divideOpeningCameraAnimationLength = 1.5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float transitionFadeToValue = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float transitionFadeToDuration = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float transitionFadeOutValue = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float transitionFadeOutDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string openingCameraAnimationName = "NSD-intro";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent openingAnimator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float openingAnimationAdditionalWaitTime = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected VolumeProfile volumeProfile = null;


        [Header("")]
        [SerializeField] protected UnityEvent onPlayIntroSound = null;


        /// <summary>
        ///
        /// </summary>
        protected DRTrialCamera trialCameraDR = null;

        /// <summary>
        ///
        /// </summary>
        protected Volume volume = null;


        protected virtual void Start()
        {
            trialCameraDR = FindAnyObjectByType<DRTrialCamera>();

            if (volumeProfile != null)
                volume = FindObjectsByType<Volume>()
                    .Where(foundVolume => foundVolume.profile.name == volumeProfile.name)
                    .FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override void ShowUI() { }

        /// <inheritdoc/>
        protected override void HideUI() { }


        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            TransitionTrialCamera();

            Fade(transitionFadeToValue, transitionFadeToDuration);

            yield return WaitForTransitionToFinish();

            DisplayOpeningAnimator(true);

            ResetTrialCameraOverrides();

            EnableVolume();

            Fade(transitionFadeOutValue, transitionFadeOutDuration);

            DisplayIntro();

            yield return new WaitForSeconds(transitionFadeOutDuration);

            DisableOverlay();

            PlayIntroSound();

            yield return new WaitForSeconds(openingAnimationAdditionalWaitTime);

            DisplayOpeningAnimator(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TransitionTrialCamera()
        {
            if (trialCameraDR == null)
                trialCameraDR = FindAnyObjectByType<DRTrialCamera>();

            trialCameraDR.TriggerAnim(transitionCameraAnimationName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="show"></param>
        protected virtual void DisplayOpeningAnimator(bool show)
        {
            if (openingAnimator == null)
                return;

            if (show)
                openingAnimator.Show();
            else
                openingAnimator.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        protected virtual void Fade(float to, float duration)
        {
            if (transitionBlackFade != null)
#if DOTWEEN
                transitionBlackFade.DOFade(to, duration);
#else
                transitionBlackFade.color = new(transitionBlackFade.color.r, transitionBlackFade.color.g, transitionBlackFade.color.b, to);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForTransitionToFinish()
        {
            float animationLength = trialCameraDR.CameraAnimator.GetAnimationClip(transitionCameraAnimationName).length;

            if (divideOpeningCameraAnimationLength > 0)
                yield return new WaitForSeconds(animationLength / divideOpeningCameraAnimationLength);
            else
                yield return new WaitForSeconds(animationLength);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetTrialCameraOverrides() => trialCameraDR.SetDefaultValues();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableVolume()
        {
            if (volume != null)
                volume.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayIntro() => trialCameraDR.TriggerAnim(openingCameraAnimationName);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableOverlay()
        {
            if (transitionBlackFade != null)
                transitionBlackFade.enabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlayIntroSound() => onPlayIntroSound?.Invoke();
    }
}