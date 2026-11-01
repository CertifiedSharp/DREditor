//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
using System.Linq;
#endif
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.DialogueEventHandling.CameraEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(ITweenWaiter) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CamToPositionEvent : DialogueEventHandler, ITweenWaiter, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent resetPlayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float moveToPositionDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float moveToRotationDuration = 1;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PathType pathType = PathType.CatmullRom;
#endif

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string dialogueEventStringCamAppend = "AppendToCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string toggleBlurEventString = "ToggleBlur";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string checkFocusEventString = "CheckFocus";

#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        protected Sequence activeCameraSequence;
#endif
        /// <summary>
        ///
        /// </summary>
        protected UnityEngine.Camera dialogueCamera;

        /// <summary>
        ///
        /// </summary>
        protected GameObject objectToMoveTo;

        /// <summary>
        ///
        /// </summary>
        protected readonly List<Vector3> stackedPaths = new();


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();
            
            DialogueEventSystem.StartListening(dialogueEventStringCamAppend, AttemptAppendTween);

            resetPlayer.RegisterListener(this);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            DialogueEventSystem.StopListening(dialogueEventStringCamAppend, AttemptAppendTween);

            resetPlayer.UnregisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => SetDefaultEventString("CamToPosition");

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => dialogueCamera = GetComponent<UnityEngine.Camera>();


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            CamToPosition data = (CamToPosition)o;

            // TO-DO: In the future find a better way to locate camera positions as this is kind of a nono
            objectToMoveTo = GameObject.Find(data.objectName);

            if (!data.keepFocus)
                TriggerUnfocusEvents();

            if (!isLoadingFile.Value && PositionIsValid(objectToMoveTo, data.objectName))
                EvaluateCameraPosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TriggerUnfocusEvents()
        {
            DialogueEventSystem.TriggerEvent(toggleBlurEventString, false);

            DialogueEventSystem.TriggerEvent(checkFocusEventString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual bool PositionIsValid(GameObject go, string name)
        {
            bool result = go != null;

            if (!result)
                UnityEngine.Debug.LogWarning($"{ToString()}: Couldn't find object \"" + name + "\" In the " +
                    "current Scene: " + SceneManager.GetActiveScene().name);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateCameraPosition()
        {
            if (inFastForwardMode.Value)
                TeleportCameraToPosition();
#if DOTWEEN
            else if (!CurrentlyMovingCamera())
                StartCameraMoveSequence();
            else
                AddPositionToCameraSequence();
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TeleportCameraToPosition() 
            => dialogueCamera.transform
            .SetPositionAndRotation(objectToMoveTo.transform.position, Quaternion.Euler(objectToMoveTo.transform.eulerAngles));
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CurrentlyMovingCamera() 
            => DOTween.IsTweening(dialogueCamera.transform) || activeCameraSequence != null && activeCameraSequence.IsPlaying();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartCameraMoveSequence()
        {
            stackedPaths.Add(dialogueCamera.transform.position);

            activeCameraSequence = DOTween.Sequence();

            activeCameraSequence.OnKill(() => activeCameraSequence = null);

            activeCameraSequence
                .Append(dialogueCamera.transform.DOMove(objectToMoveTo.transform.position, moveToPositionDuration))
                .Join(dialogueCamera.transform.DORotate(objectToMoveTo.transform.eulerAngles, moveToRotationDuration))
                .Play()
                .onComplete += ClearPathStack;

            stackedPaths.Add(objectToMoveTo.transform.position);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddPositionToCameraSequence()
        {
            stackedPaths.Add(objectToMoveTo.transform.position);

            activeCameraSequence
                .Join(dialogueCamera.transform.DOPath(stackedPaths.ToArray(), stackedPaths.Count * moveToPositionDuration, pathType))
                .Join(dialogueCamera.transform.DORotate(objectToMoveTo.transform.eulerAngles, moveToRotationDuration));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearPathStack()
        {
            activeCameraSequence.onComplete -= ClearPathStack;

            stackedPaths.Clear();
        }
#endif
        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <remarks></remarks>
        protected virtual void AttemptAppendTween(object o = null)
        {
#if DOTWEEN
            if (o is Tween tween)
                activeCameraSequence?.Append(tween);
#endif
        }


        /// <inheritdoc/>
        public virtual IEnumerator WaitForTweenToFinish()
        {
#if DOTWEEN
            if (activeCameraSequence != null && activeCameraSequence.IsActive() && activeCameraSequence.IsPlaying())
                while (activeCameraSequence != null)
#endif
                    yield return null;
        }

        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
#if DOTWEEN
            activeCameraSequence?.Kill();

            activeCameraSequence = null;
#endif
        }
    }
}