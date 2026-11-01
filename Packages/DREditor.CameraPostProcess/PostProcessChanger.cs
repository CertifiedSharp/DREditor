//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine;

#if UNITY_EDITOR

using DREditor.Editor.Utility;

#endif

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PostProcessChanger : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string setHSVEventString = "SetHSV";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureFreezeFrameEventString = "CaptureMain";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureProtagFreezeFrameEventString = "CaptureProtag";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string destroyCommandEventString = "DestroyPostProcess";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color setHSVColor = new(0.2784314f, 0.2784314f, 0.2784314f, 1);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<PostProcessState> setStates = new();


        /// <summary>
        ///
        /// </summary>
        protected readonly List<PostProcessState> originalStates = new();

        /// <summary>
        ///
        /// </summary>
        protected List<UnityEngine.Camera> cameras = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            InitializeOriginalStates();

            ApplyStateList(setStates);

            UpdateUI();

            DialogueEventSystem.StartListening(destroyCommandEventString, DestroyObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            ApplyStateList(originalStates);

            originalStates.Clear();

            DialogueEventSystem.StopListening(destroyCommandEventString, DestroyObject);

            DialogueEventSystem.TriggerEvent(destroyCommandEventString);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeOriginalStates()
        {
            cameras = FindObjectsByType<UnityEngine.Camera>().ToList();

            foreach (var state in setStates)
                if (cameras.Find(camera => camera.gameObject.name == state.cameraObjectName) is UnityEngine.Camera found)
                    originalStates.Add(ScriptableObject.CreateInstance<PostProcessState>().RecordState(found));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="states"></param>
        protected virtual void ApplyStateList(List<PostProcessState> states)
        {
            if (cameras == null)
                return;

            foreach (var camera in cameras)
                if (camera != null && states.Find(state => state.cameraObjectName == camera.gameObject.name) is PostProcessState state)
                    state.ApplyState(camera);
                else if (camera != null)
                    UnityEngine.Debug.LogWarning("ALERT: Could not find state data for camera: " + camera.name);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateUI()
        {
            DialogueEventSystem.TriggerEvent(setHSVEventString, setHSVColor);

            if (inDialogue.Value)
                ScreenshotCameras();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void ScreenshotCameras()
        {
            DialogueEventSystem.TriggerEvent(captureFreezeFrameEventString, true);

            DialogueEventSystem.TriggerEvent(captureProtagFreezeFrameEventString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void DestroyObject(object o) => StartCoroutine(DestroyMidDialogueRoutine());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DestroyMidDialogueRoutine()
        {
            if (TryGetComponent(out Volume volume))
                Destroy(volume);

            yield return null;

            ScreenshotCameras();

            yield return null;

            Destroy(gameObject);
        }


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Set all States to True")]
        public void EnableAllSet()
        {
            foreach (var postProcessState in setStates)
                postProcessState.SetEnabled(true);
        }

#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Create PostProcess Assets from all Cameras in Scene")]
        public void Scan()
        {
            UnityEngine.Debug.Log($"{ToString()}: Scans the scene for all cameras and creates PostProcessState assets for each.");

            var cameras = FindObjectsByType<UnityEngine.Camera>();

            setStates.Clear();

            foreach (var camera in cameras)
            {
                var state = CustomEditorUtility.CreateAsset<PostProcessState>("PostProcessState " + camera.gameObject.name);

                state = state.RecordState(camera);

                setStates.Add(state);
            }
        }

#endif
    }
}