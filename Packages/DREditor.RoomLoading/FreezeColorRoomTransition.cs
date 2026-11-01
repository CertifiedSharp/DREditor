//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues.Events;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    public class FreezeColorRoomTransition : DRERoomTransition
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected Canvas freezeCanvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage freezeImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage freezeBack = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected float freezeImageStartDuration = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float freezeBackStartDuration = 2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float freezeImageEndDuration = 2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float freezeBackEndDuration = 1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureCameraEventString = "CaptureMainCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string enableMainCameraEventString = "EnableMainCamera";


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator StartFreezeColorRoutine()
        {
            isTransitioning = true;

            // NOTE: This code line was added 8/20/2024 to fix ch1 investigation, going to 
            // the courtyard to the tree room has the main capture image still be the courtyard.
            // however if this fix ends up causing more problems, a different method to fix this 
            // should be used.

            if (ServiceLocator.GetService<GlobalFade>().IsDark)
            {
                isTransitioning = false;

                yield break;
            }

            RenderFreeze();

            yield return null;

            UnityEngine.Debug.Log($"{ToString()}: Changing color...");

            Color background = new(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));
#if DOTWEEN
            freezeImage.DOColor(background, freezeImageStartDuration);

            freezeBack.DOColor(background, freezeBackStartDuration);
#else
            freezeImage.color = background;

            freezeBack.color = background;
#endif
            yield return new WaitUntil(() => freezeBack.color == background);

            isTransitioning = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RenderFreeze()
        {
            UnityEngine.Debug.Log($"{ToString()}: Capturing Screen to freeze.");

            DialogueEventSystem.TriggerEvent(captureCameraEventString, freezeImage);

            DialogueEventSystem.TriggerEvent(enableMainCameraEventString);

            freezeCanvas.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator EndFreezeColorRoutine()
        {
            isTransitioning = true;

            if (ServiceLocator.GetService<GlobalFade>().IsDark)
            {
                isTransitioning = false;

                yield break;
            }
#if DOTWEEN
            freezeImage.DOFade(0, freezeImageEndDuration);

            freezeBack.DOFade(0, freezeBackEndDuration);
#else
            freezeImage.color = new(freezeImage.color.r, freezeImage.color.g, freezeImage.color.b, 0);

            freezeBack.color = new(freezeBack.color.r, freezeBack.color.g, freezeBack.color.b, 0);
#endif

            UnityEngine.Debug.Log($"{ToString()}: Starting Fading out room color");

            yield return new WaitUntil(() => freezeImage.color.a == 0);

            UnityEngine.Debug.Log($"{ToString()}: Finished Fading out room color");

            freezeCanvas.enabled = false;

            freezeImage.color = new Color(1, 1, 1, 1);

            isTransitioning = false;
        }


        /// <inheritdoc/>
        public override IEnumerator StartTransitionRoutine() => StartFreezeColorRoutine();


        /// <inheritdoc/>
        public override IEnumerator EndTransitionRoutine() => EndFreezeColorRoutine();
    }
}