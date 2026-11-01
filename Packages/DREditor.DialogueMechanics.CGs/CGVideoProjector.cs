//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class CGVideoProjector : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(InDebugMode))]
        [Tooltip("")]
        [SerializeField] protected bool debugBreak = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGStateMode = null;

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
        [Tooltip("")]
        [SerializeField] protected RawImage videoBack = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float videoToOtherCGFadeTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startVideoInCGTransitionTime = 0.5f;


        /// <summary>
        ///
        /// </summary>
        /// <param name="iniClip"></param>
        /// <param name="mainClip"></param>
        /// <param name="isLooping"></param>
        /// <param name="playbackSpeed"></param>
        /// <param name="fadeOutCallback"></param>
        /// <param name="otherActive"></param>
        public virtual IEnumerator PlayStartingVideoTransition(VideoClip iniClip, VideoClip mainClip,
            bool isLooping, float playbackSpeed, IEnumerator fadeOutCallback, bool otherActive = false)
        {
            Debug("PlayStartingVideoTransition");
            
            VideoClip clip = iniClip != null ? iniClip : mainClip;

            PrepareStartingVideo(clip, isLooping, 1, otherActive);

            yield return PlayOnPrepared();

            if (!otherActive)
                FadeVideoImages(1, startVideoInCGTransitionTime);

            videoPlayer.playbackSpeed = 0;

            yield return fadeOutCallback;

            videoPlayer.playbackSpeed = playbackSpeed;

            if (iniClip != null)
                yield return QueueNextClip(mainClip, isLooping);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isLooping"></param>
        /// <param name="playbackSpeed"></param>
        /// <param name="otherActive"></param>
        protected virtual void PrepareStartingVideo(VideoClip clip, bool isLooping, float playbackSpeed, bool otherActive = false)
        {
            Debug($"PrepareStartingVideo {otherActive}");
            
            inCGStateMode.SetValue(true);
#if DOTWEEN
            if (otherActive)
                videoBack.DOFade(1, 0);
#endif
            SetVideoPlayer(clip, isLooping, playbackSpeed, new(Screen.width, Screen.height, 32));

            videoProjector.texture = videoPlayer.targetTexture;
#if DOTWEEN
            if (otherActive)
                videoProjector.DOFade(1, 0);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator PlayOnPrepared()
        {
            Debug("PlayOnPrepared");
            
            yield return new WaitUntil(() => videoPlayer.isPrepared);

            videoPlayer.Play();

            yield return new WaitUntil(() => videoPlayer.frame >= 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="duration"></param>
        protected virtual void FadeVideoImages(float visible, float duration)
        {
            Debug("FadeVideoImages");
#if DOTWEEN
            videoBack.DOFade(visible, duration);

            videoProjector.DOFade(visible, duration);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isLooping"></param>
        protected virtual IEnumerator QueueNextClip(VideoClip clip, bool isLooping)
        {
            Debug("QueueNextClip");
            
            yield return new WaitUntil(() => videoPlayer.frame == (long)videoPlayer.frameCount - 1);

            PrepareStartingVideo(clip, isLooping, 1);

            yield return PlayOnPrepared();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isLooping"></param>
        /// <param name="playbackSpeed"></param>
        public virtual IEnumerator PlayWhileInVideo(VideoClip clip, bool isLooping, float playbackSpeed)
        {
            Debug("PlayWhileInVideo");

            SetVideoPlayer(clip, isLooping, 0, new(Screen.width, Screen.height, 32));

            yield return PlayOnPrepared();

            RenderTexture renderTexture = new(Screen.width, Screen.height, 32);

            videoPlayer.targetTexture = renderTexture;

            videoPlayer.playbackSpeed = playbackSpeed;

            yield return null;

            videoProjector.texture = renderTexture;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isLooping"></param>
        /// <param name="playbackSpeed"></param>
        /// <param name="renderTexture"></param>
        protected virtual void SetVideoPlayer(VideoClip clip, bool isLooping, float playbackSpeed, RenderTexture renderTexture)
        {
            Debug("SetVideoPlayer");

            if (videoPlayer.targetTexture != null)
                videoBack.texture = videoPlayer.targetTexture;
            else
                videoBack.texture = renderTexture;

            videoPlayer.targetTexture = renderTexture;

            videoPlayer.isLooping = isLooping;

            videoPlayer.playbackSpeed = playbackSpeed;

            videoPlayer.clip = clip;

            if (clip != null)
                videoPlayer.Prepare();
            else
                videoPlayer.Stop();
        }


        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator WaitForVideoToFinish()
        {
            Debug("WaitForVideoToFinish");
            
            yield return new WaitUntil(() => !videoPlayer.isPlaying);

            videoPlayer.frame = (long)Mathf.Lerp(videoPlayer.frame, videoPlayer.clip.frameCount, 1);

            videoPlayer.frame = (long)videoPlayer.clip.frameCount;
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void DisableVid()
        {
            Debug("DisableVid");
            
            FadeVideoImages(0, videoToOtherCGFadeTime);

            SetVideoPlayer(null, false, 1, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        protected virtual void Debug(string message)
        {
            if (InDebugMode())
                UnityEngine.Debug.LogWarning($"REPORT: {message}");

            if (InDebugMode() && debugBreak)
                UnityEngine.Debug.Break();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InDebugMode() => debug;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool VideoVisible() => videoBack.color.a == 1;
    }
}