//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Shader That made this possible + Idea by LeoTheDev
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.Rendering.Universal;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(GameplayOverlay) + ", " + nameof(IOverlayManager) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class ObserveManager : GameplayOverlay
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent changingObserve = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent reticleCanSelect = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string toggleCameraEvent = "ToggleCameraFlags";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RenderObjects renderObject = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UniversalRendererData rendererData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LayerMask layerMask = 131073; // Default + RaycastBlock


        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent OnPlaySFX = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnStopSFX = null;


        /// <summary>
        ///
        /// </summary>
        protected IEndDialogueEvent EndDialogueEvent;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            EndDialogueEvent ??= FindObjectsByType<MonoBehaviour>().OfType<IEndDialogueEvent>().FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += ModeChanged;
#endif
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= ModeChanged;
#endif
        }

        
        /// <inheritdoc/>
        protected override void EvaluateOverlay(CallbackContext context)
        {
            if (!changingObserve.Value && displayOverlayCondition.Evaluate())
            {
                reticleCanSelect.SetValue(false);

                StartCoroutine(ToggleOverlay());
            }
        }

        /// <inheritdoc/>
        protected override IEnumerator ToggleOverlay()
        {
            changingObserve.SetValue(true);

            if (!isLoadingFile.Value)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeToTime);

            SetOverlay(!overlaying);

            DialogueEventSystem.TriggerEvent(toggleCameraEvent, overlaying);

            if (!isLoadingFile.Value)
                yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(fadeOutTime);

            if (!inDialogue.Value)
                reticleCanSelect.SetValue(true);

            changingObserve.SetValue(false);
        }


        /// <inheritdoc/>
        protected override void ClearOverlay()
        {
            base.ClearOverlay();

            if (inLeaveProcess.Value)
                EndDialogueEvent.OnDialogueEnd += FixReticleIssue;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FixReticleIssue()
        {
            EndDialogueEvent.OnDialogueEnd -= FixReticleIssue;

            reticleCanSelect.SetValue(true);
        }


        /// <inheritdoc/>
        public override void SetOverlay(bool to)
        {
            if (to)
                TurnOnOverlay();
            else
                TurnOffOverlay();

            base.SetOverlay(to);
        }

        /// <inheritdoc/>
        protected override void TurnOnOverlay()
        {
            renderObject.SetActive(true);

            rendererData.opaqueLayerMask &= ~layerMask;

            OnPlaySFX?.Invoke();
        }

        /// <inheritdoc/>
        protected override void TurnOffOverlay()
        {
            renderObject.SetActive(false);

            rendererData.opaqueLayerMask |= layerMask;

            OnStopSFX?.Invoke();
        }


        /// <inheritdoc/>
        public override void ToTitleScreen()
        {
            EndDialogueEvent.OnDialogueEnd -= FixReticleIssue;

            base.ToTitleScreen();
        }


#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <param name="change"></param>
        protected virtual void ModeChanged(PlayModeStateChange change)
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
            {
                if (renderObject.isActive)
                    renderObject.SetActive(false);

                rendererData.opaqueLayerMask |= layerMask;
            }
        }
#endif
    }
}