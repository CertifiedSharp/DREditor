//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class TPFDMoveInDialogueHandler : DialogueEventHandler, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string postEventString = "PostToTPFDFromDialogue";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string captureFreezeFrameEventString = "CaptureMain";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float cameraPositionThreshold = 0.01f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float timeOutDuration = 3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedRoomTransitions = null;


        /// <summary>
        ///
        /// </summary>
        protected bool finishedLoadingRoom = false;


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            onFinishedRoomTransitions.UnregisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => StartCoroutine(MoveToTPFDEvent());

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual IEnumerator MoveToTPFDEvent()
        {
            onFinishedRoomTransitions.RegisterListener(this);

            onFinishedLoadingRoom.RegisterListener(this);

            SetTPFDManagerValues();

            yield return WaitForCameraToSettle();

            yield return WaitForRoomTransitionsFinished();

            onFinishedRoomTransitions.UnregisterListener(this);

            onFinishedLoadingRoom.UnregisterListener(this);

            InvokePostListeners();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForCameraToSettle()
        {
            var pnc = FindAnyObjectByType<PnCCamera>();

            var mainCamera = GameObject.FindGameObjectWithTag(mainCameraTag);

            while (Vector3.Distance(mainCamera.transform.position, pnc.GetFinalPosition()) > cameraPositionThreshold)
                yield return null;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForRoomTransitionsFinished()
        {
            float timer = 0;

            while (!finishedLoadingRoom && timer <= timeOutDuration)
            {
                timer += Time.deltaTime;

                yield return null;
            }

            if (timer >= timeOutDuration)
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(TPFDMoveInDialogueHandler)}: Transition timed out! Forcing resolution.");

            finishedLoadingRoom = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetTPFDManagerValues()
        {
            if (GetActiveTPFDManager() is TPFDManagerBase tPFDManager)
                tPFDManager.StartEarlySet();
            else
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(TPFDMoveInDialogueHandler)}: " +
                    $"SetCamTPFD Function called but there is no enabled TPFDManager in the scene.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual TPFDManagerBase GetActiveTPFDManager()
        {
            foreach (var manager in FindObjectsByType<TPFDManagerBase>())
                if (manager.enabled)
                    return manager;

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected void InvokePostListeners()
        {
            DialogueEventSystem.TriggerEvent(postEventString);

            DialogueEventSystem.TriggerEvent(captureFreezeFrameEventString, true);
        }


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ToTPFDFromDialogue");


        /// <inheritdoc/>
        public virtual void OnEventRaised() => finishedLoadingRoom = true;
    }
}