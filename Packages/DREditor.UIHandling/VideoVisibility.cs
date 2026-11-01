//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) +
        "\n\n")]
    public class VideoVisibility : UIVisibilityComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Canvas videoCanvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected VideoPlayer videoPlayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage videoProjector = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float showFadeDuration = 0.2f;


        [Header("")]
        [SerializeField] protected UnityEvent OnVideoStarted;


        /// <inheritdoc/>
        protected override void ShowUI() => StartCoroutine(ShowRoutine());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowRoutine()
        {
            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(showFadeDuration);

            videoCanvas.enabled = true;

            videoPlayer.Prepare();

            yield return new WaitUntil(() => videoPlayer.isPrepared);

            videoPlayer.Play();

            yield return new WaitUntil(() => videoPlayer.isPlaying);

            yield return new WaitUntil(() => videoPlayer.frame >= 1);

            RenderTexture renderTexture = new(Screen.width, Screen.height, 32);

            videoProjector.texture = renderTexture;

            videoPlayer.targetTexture = renderTexture;

            OnVideoStarted?.Invoke();

            yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(showFadeDuration);
        }

        /// <inheritdoc/>
        protected override void HideUI()
        {
            videoPlayer.Stop();

            videoCanvas.enabled = false;
        }

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            yield return new WaitUntil(() => !videoPlayer.isPlaying);
        }

        /// <inheritdoc/>
        public override IEnumerator ShowTillComplete()
        {
            yield return base.ShowTillComplete();

            yield return new WaitUntil(() => videoPlayer.isPlaying);

            yield return new WaitUntil(() => !videoPlayer.isPlaying);
        }
    }
}