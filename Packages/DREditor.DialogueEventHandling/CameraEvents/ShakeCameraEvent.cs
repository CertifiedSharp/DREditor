//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.Services;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(ITweenWaiter) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ShakeCameraEvent : DialogueEventHandler, ITweenWaiter, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera dialogueCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera protagCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float stopCamReturnDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera targetCamera;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 lastPos;

        /// <summary>
        ///
        /// </summary>
        protected ILineReader lineReader;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected Tweener shakeTweener;
#endif

        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => SetDefaultEventString("ShakeCamera");

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => lineReader = FindObjectsByType<MonoBehaviour>().OfType<ILineReader>().FirstOrDefault();


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            ShakeObject data = (ShakeObject)o;

            targetCamera = data.protag? protagCamera : dialogueCamera;
#if DOTWEEN
            if (DOTween.IsTweening(targetCamera.transform))
                StartCoroutine(ShakeWait(data));
            else
                ShakeCameraProcess(data);
#else
            ShakeCameraProcess(data);
#endif
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        protected virtual IEnumerator ShakeWait(ShakeObject data)
        {
            UnityEngine.Debug.Log("REPORT: Camera Shake is waiting to play.");

            yield return new WaitUntil(() => !DOTween.IsTweening(targetCamera.transform));

            UnityEngine.Debug.Log("REPORT: Camera Shake has finished waiting.");

            ShakeCameraProcess(data);
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        protected virtual void ShakeCameraProcess(ShakeObject data)
        {
            lastPos = targetCamera.transform.position;

            lineReader.OnLineEndEvent += StopCameraShaking;
#if DOTWEEN
            if (targetCamera != null)
                shakeTweener = targetCamera
                    .DOShakePosition(data.duration, data.strength / 50, data.vibrato, data.randomness, data.fadeOut);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopCameraShaking()
        {
            lineReader.OnLineEndEvent -= StopCameraShaking;
#if DOTWEEN
            shakeTweener?.Kill();

            if (targetCamera != null)
                targetCamera.transform.DOMove(lastPos, stopCamReturnDuration);
#endif
        }


        /// <inheritdoc/>
        public virtual IEnumerator WaitForTweenToFinish()
        {
            while (ShouldWaitForTween())
                yield return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldWaitForTween() =>
#if DOTWEEN
            CameraIsTweening(dialogueCamera) || CameraIsTweening(protagCamera);

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        protected virtual bool CameraIsTweening(UnityEngine.Camera camera) => camera != null && DOTween.IsTweening(camera.transform);
#else
            true;
#endif

        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            StopCameraShaking();
#if DOTWEEN
            if (targetCamera != null)
                targetCamera.DOKill();
#endif
        }
    }
}