//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<BlurCameraRenderManager>) + " for type: " + nameof(BlurCameraRenderManager) +
        "\n\n")]
    public class BlurCameraRenderManager : ServiceHolder<BlurCameraRenderManager>, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool lowSpecCamRender = false;

        /// <summary>
        ///
        /// </summary>
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected List<string> renderingCodes = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected StringWithEvent blurCamCode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent blurCameraEnabled = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] UnityEngine.Camera dialogueCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueCameraName = "Dialogue";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string renderCamCode = "Cam";


        /// <summary>
        ///
        /// </summary>
        protected bool mainCameraIsTweening = false;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            renderingCodes.RemoveAll(string.IsNullOrWhiteSpace);

            InitializeMissingCamera(ref dialogueCamera, dialogueCameraName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="identifier"></param>
        protected virtual void InitializeMissingCamera(ref UnityEngine.Camera camera, string identifier)
        {
            if (camera == null)
                camera = FindObjectsByType<UnityEngine.Camera>()
                    .Where(camera => camera.gameObject.name.Contains(identifier))
                    .FirstOrDefault();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => blurCamCode.AddListener(EvaluateCameraCode);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => blurCamCode.RemoveListener(EvaluateCameraCode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="renderCode"></param>
        protected virtual void EvaluateCameraCode(string renderCode)
        {
            if (!renderingCodes.Contains(renderCode))
                AddRenderQueue(renderCode);
            else
                RemoveRenderQueue(renderCode);
        }
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected virtual void Update()
        {
            if (lowSpecCamRender)
                CheckIfCamerasTweening();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckIfCamerasTweening()
        {
            if (DOTween.IsTweening(dialogueCamera.transform) && !mainCameraIsTweening)
            {
                AddRenderQueue(renderCamCode);

                mainCameraIsTweening = true;
            }
            else if (!DOTween.IsTweening(dialogueCamera.transform) && mainCameraIsTweening)
            {
                RemoveRenderQueue(renderCamCode);

                mainCameraIsTweening = false;
            }
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="queueCode"></param>
        public virtual void AddRenderQueue(string queueCode)
        {
            if (!lowSpecCamRender || string.IsNullOrEmpty(queueCode))
                return;

            if (renderingCodes.Count == 0)
                blurCameraEnabled.SetValue(true);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Adding to render queue" +  queueCode);

            renderingCodes.Add(queueCode);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="queueCode"></param>
        public virtual void RemoveRenderQueue(string queueCode)
        {
            if (!lowSpecCamRender || string.IsNullOrEmpty(queueCode))
                return;

            renderingCodes.Remove(queueCode);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Removing from render queue" + queueCode);
#if DOTWEEN
            if (renderingCodes.Count == 0 && !DOTween.IsTweening(dialogueCamera.transform))
#else
            if (renderingCodes.Count == 0)
#endif
                blurCameraEnabled.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool UsingLowSpecCamRender() => lowSpecCamRender;

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        public virtual void SetLowSpecCamRender(bool to) => lowSpecCamRender = to;


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => ClearRenderQ();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearRenderQ()
        {
            renderingCodes.Clear();

            blurCameraEnabled.SetValue(false);
        }
    }
}