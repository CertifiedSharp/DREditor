//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Finishing;
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPreparableDialogueComponent) + ", " + nameof(IResolvableDialogueComponent) + ", "
        + nameof(IFinishableDialogueComponent) +
        "\n\n")]
    public class LookAtObjectOnSelection : DialogueComponent, IPreparableDialogueComponent, IResolvableDialogueComponent,
         IFinishableDialogueComponent, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float prepareOrder = 1;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveOrder = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float finishOrder = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int finishPreparerID = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;

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
        [SerializeField] protected string dialogueCameraToMainEventString = "DialogueCameraToMainCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float lookAtPlayerTime = 1;


        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected bool skipOGCamLook = false;

        /// <summary>
        ///
        /// </summary>
        protected Quaternion originalCameraLookRotation;


        float IDialogueComponent<IPreparableDialogueComponent>.Order { get => prepareOrder; }

        float IDialogueComponent<IResolvableDialogueComponent>.Order { get => resolveOrder; }

        float IDialogueComponent<IFinishableDialogueComponent>.Order { get => finishOrder; }

        int IDialogueComponent<IFinishableDialogueComponent>.ExclusiveHandlerID => finishPreparerID;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => DialogueEventSystem.StartListening("PostToTPFDFromDialogue", OnCameraSet);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueEventSystem.StopListening("PostToTPFDFromDialogue", OnCameraSet);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void OnCameraSet(object o = null) => SetOGLookToMainCamera();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetOGLookToMainCamera() 
            => originalCameraLookRotation = GameObject.FindWithTag(mainCameraTag).transform.rotation;

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableSkipOGCameraLook() => skipOGCamLook = false;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ReturnToOriginalPosition()
        {
            if (!skipOGCamLook)
#if DOTWEEN
                GameObject.FindWithTag(mainCameraTag).transform.DORotateQuaternion(originalCameraLookRotation, lookAtPlayerTime);
#else
                GameObject.FindWithTag(mainCameraTag).transform.Rotate(originalCameraLookRotation.eulerAngles);
#endif
            else
                DisableSkipOGCameraLook();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckSkippingOGCam()
        {
            if (skipOGCamLook)
                DisableSkipOGCameraLook();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator LookAtSelectedObjectProcess()
        {
            Talkable selectedObject = ServiceLocator.GetService<DialogueObjectManager>().GetStartObject() as Talkable;

            PrepareCameraPositionsForDialogueStart();

            if (!inTPFD.Value)
                LookAtHead(selectedObject);

            if (!isLoadingFile.Value)
                yield return new WaitForSeconds(lookAtPlayerTime);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PrepareCameraPositionsForDialogueStart()
        {
            if (isLoadingFile.Value)
                EnableSkipOGCameraLook();
            else
                DialogueEventSystem.TriggerEvent(dialogueCameraToMainEventString);

            SetOGLookToMainCamera();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableSkipOGCameraLook() => skipOGCamLook = true;

        /// <summary>
        ///
        /// </summary>
        /// <param name="talkable"></param>
        protected virtual void LookAtHead(Talkable talkable)
        {
            if (talkable == null)
                return;

            if (!talkable.head)
                UnityEngine.Debug.LogWarning("ALERT: " + talkable.gameObject.name + " doesn't have a head initialized.");
            else
#if DOTWEEN
                GameObject.FindWithTag(mainCameraTag).transform.DOLookAt(talkable.head.position, lookAtPlayerTime);
#else
                GameObject.FindWithTag(mainCameraTag).transform.LookAt(talkable.head.position);
#endif
        }


        /// <inheritdoc/>
        public virtual bool UseRoutineToPrepare() => true;

        /// <inheritdoc/>
        public virtual void Prepare() => StartCoroutine(PrepareRoutine());

        /// <inheritdoc/>
        public virtual IEnumerator PrepareRoutine() => LookAtSelectedObjectProcess();


        /// <inheritdoc/>
        public virtual bool UseRoutineToResolve() => false;

        /// <inheritdoc/>
        public virtual void Resolve() => ReturnToOriginalPosition();

        /// <inheritdoc/>
        public virtual IEnumerator ResolveRoutine()
        {
            Resolve();

            yield break;
        }


        /// <inheritdoc/>
        public virtual void OnFinishDialogue() => CheckSkippingOGCam();


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => DisableSkipOGCameraLook();
    }
}