//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using DREditor.TwoPointFiveD;
using System.Collections;
using UnityEngine;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(SceneEventListener))]
    [ClassHeader(nameof(SubAreaActivationHandler) + ", " + nameof(IDialogueHolder) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class SubAreaActivator : SubAreaActivationHandler, IDialogueHolder, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeTime = 0.3f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SubAreaActivationHandler deactivator;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObject focusedActor = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Dialogue startDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Dialogue autoStartDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject cameraArrowsPrefab = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent reticleCanSelect = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inSubAreaActivation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent OnSAMActivate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent OnSAMAutoStartDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadingOutOfLoading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedRoomTransitions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SubAreaData Data;


        /// <summary>
        ///
        /// </summary>
        protected GameObject cameraArrowsRef;

        /// <summary>
        ///
        /// </summary>
        protected bool cachedLoadingSave;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (deactivator != null)
                deactivator.OnHandlerFinished += DestroyCameraArrows;
            
            onFinishedRoomTransitions.Response.AddListener(CheckLoad);

            cachedLoadingSave = isLoadingFile.Value;

            onFadingOutOfLoading.RegisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (deactivator != null)
                deactivator.OnHandlerFinished -= DestroyCameraArrows;

            onFinishedRoomTransitions.Response.RemoveListener(CheckLoad);

            if (Data != null && Data.isWaiting)
                UnsubscribeEndDialogueEvent(ActivateSubAreaOnDialogueEnd);

            if (Data != null && Data.isWaitingAuto)
                UnsubscribeEndDialogueEvent(EndOfActivationHandler);

            onFadingOutOfLoading.UnregisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckLoad()
        {
            if (Data.isWaiting || Data.isWaitingAuto)
                inSubAreaActivation.SetValue(true);

            if (Data.isWaiting)
                SubscribeEndDialogueEvent(ActivateSubAreaOnDialogueEnd);

            if (Data.isWaitingAuto)
            {
                UnityEngine.Debug.Log(ToString() + ": Is Waiting Auto Was True");

                SubscribeEndDialogueEvent(EndOfActivationHandler);

                Activate();
            }
        }


        /// <inheritdoc/>
        public override void Initialize(object subAreaData) => Data = (SubAreaData)subAreaData;

        /// <inheritdoc/>
        public override void BeginActivationState()
        {
            inSubAreaActivation.SetValue(true);

            if (startDialogue != null && !Data.finishedStart)
            {
                TurnOffDisplayedObjectName();

                dialogueManager.StartDialogue(focusedActor, startDialogue);

                Data.SetIsWaiting(true);

                SubscribeEndDialogueEvent(ActivateSubAreaOnDialogueEnd);

                UnityEngine.Debug.Log("Start Dialogue and Data hasn't finished start");

                return;
            }
            else
            {
                UnityEngine.Debug.Log("Activation else");

                Activate();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateSubAreaOnDialogueEnd()
        {
            UnsubscribeEndDialogueEvent(ActivateSubAreaOnDialogueEnd);

            //RaycastReticle.Instance.FadeOut();
            if (autoStartDialogue && !Data.finishedAuto)
            {
                //RaycastReticle.Instance.FadeOut();
                OnSAMAutoStartDialogue.Raise();

                inDialogue.Value = true;
            }

            Data.FinishedStart();

            Activate();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Activate()
        {
            bool loadingIsWaitingAuto = Data.isWaitingAuto;

            if (!isLoadingFile.Value)
                Data.SetReturnValues(mainCamera);

            subAreaCollider.enabled = false;

            OnSAMActivate.Raise();

            TurnOffDisplayedObjectName();

            hasMenuAccess.SetValue(false);

            playerSystem.EnableBehaviours(false);

            playerSystem.enabled = false;

            if (AlreadyInTPFD())
                GetTPFDDoors().ForEach(door => door.RemoveDoorControl());

            if (loadingIsWaitingAuto)
                StartCoroutine(SaveLoadActivation());
            else
                StartCoroutine(Activation());

            if (!AlreadyInTPFD())
            {
                cameraArrowsRef = Instantiate(cameraArrowsPrefab);

                tpfdManager.InitializeTPFD();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SaveLoadActivation()
        {
            yield return new WaitUntil(() => !isLoadingFile.Value);

            yield return MoveToSubAreaPosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator Activation()
        {
            if (startDialogue && autoStartDialogue && !Data.finishedAuto)
                yield return new WaitUntil(() => hasMenuAccess.Value == false);

            hasMenuAccess.SetValue(false);

            if (AlreadyInTPFD() && !ServiceLocator.GetService<GlobalFade>().IsDark)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeTime);

            yield return MoveToSubAreaPosition();

            if (AlreadyInTPFD() && ServiceLocator.GetService<GlobalFade>().IsDark && !cachedLoadingSave)
                yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(fadeTime);

            EndOfActivationHandler();
        }

        /// <inheritdoc/>
        public override void EndOfActivationHandler()
        {
            if (autoStartDialogue && !Data.isWaitingAuto && !Data.finishedAuto)
            {
                dialogueManager.StartDialogue(focusedActor, autoStartDialogue);

                Data.SetIsWaitingAuto(true);

                SubscribeEndDialogueEvent(EndOfActivationHandler);

                return;
            }

            if (autoStartDialogue && Data.isWaitingAuto)
            {
                UnsubscribeEndDialogueEvent(EndOfActivationHandler);

                Data.FinishedAuto();
            }

            inTPFD.Value = true;

            playerSystem.TPFDScript.enabled = true;

            ((PnCCamera)playerSystem.TPFDScript).Controllable = true;

            playerSystem.RaycastScript.enabled = true;

            if (!reticleCanSelect.Value)
                reticleCanSelect.SetValue(true);

            objectGroup.EnableItems(true);

            hasMenuAccess.SetValue(true);

            Data.SetActivated(true);

            FinishHandler();

            deactivator.BeginActivationState();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator MoveToSubAreaPosition()
        {
            PnCCamera pnc = (PnCCamera)playerSystem.TPFDScript;

            pnc.ClearPosition();

            if (AlreadyInTPFD())
            {
                foreach (TPFDManager manager in managers)
                    manager.enabled = false;

                tpfdManager.enabled = true;

                tpfdManager.SetPnCCamera();
            }

            float duration = isLoadingFile.Value || AlreadyInTPFD() ? 0 : panToBodyTime;

            yield return MoveCameraToPosition(pnc.GetCalculatedPosition(true), Vector3.zero, duration);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroyCameraArrows()
        {
            if (!AlreadyInTPFD())
                Destroy(cameraArrowsRef);
        }


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
            => ServiceLocator.GetService<IDialogueManager>()
            .GetManagerComponent<DialogueLoadIdentifier>()
            .CheckDialogue<Line>(startDialogue)
            ||
            ServiceLocator.GetService<IDialogueManager>()
            .GetManagerComponent<DialogueLoadIdentifier>()
            .CheckDialogue<Line>(autoStartDialogue);

        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            if (cachedLoadingSave)
                cachedLoadingSave = false;
        }
    }
}