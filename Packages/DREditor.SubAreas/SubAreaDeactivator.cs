//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Choices;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Doors;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Input;
using DREditor.Services;
using DREditor.TwoPointFiveD;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SubAreaActivationHandler) +
        "\n\n")]
    public class SubAreaDeactivator : SubAreaActivationHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float panToBodyBufferTime = 0.1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float leaveAskAnimationDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string onDidntLeaveEvent = "SubAreaOnDidntLeave";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inSubAreaActivation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent OnSAMDeactivateVisuals = null;


        /// <summary>
        ///
        /// </summary>
        protected Action ResubscribeLeaveInput;

        /// <summary>
        ///
        /// </summary>
        protected SubAreaData Data;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            ResubscribeLeaveInput = () => ServiceLocator.GetService<IInputManager>().GetLeave().performed += Release;
        }

        protected virtual void OnDestroy() => ServiceLocator.GetService<IInputManager>().GetLeave().performed -= Release;

        /// <inheritdoc/>
        public override void Initialize(object subAreaData) => Data = (SubAreaData)subAreaData;
        
        /// <inheritdoc/>
        public override void BeginActivationState()
        {
            inSubAreaActivation.SetValue(false);

            ResubscribeLeaveInput?.Invoke();
        }

        #region Leave Input

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Release(CallbackContext context)
        {
            ServiceLocator.GetService<IInputManager>().GetLeave().performed -= Release;

            if (!inLeaveProcess.Value && !inMenu.Value && !inDialogue.Value)
                CheckRelease();
            else
                ResubscribeLeaveInput?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckRelease()
        {
            SetAccessibility(false);

            if (!Data.leaveData.IsUnlocked())
                OnLeavingLocked();
            else
                PromptLeavingSubArea();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnLeavingLocked()
        {
            if (TryPlayingDialogue(null, Data.leaveData.lockDialogues))
                SubscribeEndDialogueEvent(AfterLock);
            else
                PromptLeavingSubArea();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PromptLeavingSubArea()
        {
            SubscribeChoices();

            Dialogue dialogue = (Dialogue)ServiceLocator.GetService<LeaveAskManager>().GetLeaveAskDialogue();

            dialogueManager.StartDialogue(null, dialogue, true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeChoices()
        {
            ServiceLocator.GetService<IChoiceManager>().SubscribeChoice(0, Deactivate);

            ServiceLocator.GetService<IChoiceManager>().SubscribeChoice(1, DummyNoOption);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DummyNoOption()
        {
            UnsubscribeChoices();

            EventSystem.current.SetSelectedGameObject(null);

            DialogueEventSystem.TriggerEvent(onDidntLeaveEvent);

            AfterLock();

            ResubscribeLeaveInput?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeChoices()
        {
            ServiceLocator.GetService<IChoiceManager>().UnsubscribeChoice(0, Deactivate);

            ServiceLocator.GetService<IChoiceManager>().UnsubscribeChoice(1, DummyNoOption);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void AfterLock()
        {
            UnsubscribeEndDialogueEvent(AfterLock);

            SetAccessibility(true);

            ResubscribeLeaveInput?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetAccessibility(bool to)
        {
            inLeaveProcess.SetValue(!to);

            playerSystem.TPFDScript.enabled = to;

            hasMenuAccess.SetValue(to);
        }

        #endregion

        #region Deactivation

        /// <summary>
        ///
        /// </summary>
        protected virtual void Deactivate()
        {
            UnsubscribeChoices();

            StartCoroutine(Deactivation());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator Deactivation()
        {
            if (AlreadyInTPFD())
                yield return DeactivateTPFD();
            else
                yield return DeactivateThreeD();

            OnSAMDeactivateVisuals.Raise();

            TurnOffDisplayedObjectName();

            objectGroup.EnableItems(false);

            if (AlreadyInTPFD())
                yield return PreDeactivateTPFD();

            EndOfActivationHandler();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DeactivateTPFD()
        {
            if (!ServiceLocator.GetService<GlobalFade>().IsDark)
            {
                yield return new WaitForSeconds(leaveAskAnimationDuration);

                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeTime);
            }

            tpfdManager.enabled = false;

            foreach (TPFDManager manager in managers)
            {
                manager.enabled = true;

                manager.SetPnCCamera();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DeactivateThreeD()
        {
            playerSystem.EnableBehaviours(false);

            playerSystem.enabled = false;

            inTPFD.Value = false;

            SmoothMouseLook mouse = mainCamera.GetComponent<SmoothMouseLook>();

            mouse.SetCorePos(Data.returnAbs); // was new Vector2(1.401298e-45f, 1.401298e-45f)

            yield return MoveCameraToPosition(Data.returnPosition, Data.returnRotation, panToBodyTime, panToBodyBufferTime);

            playerSystem.enabled = true;

            playerSystem.EnableBehaviours(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator PreDeactivateTPFD()
        {
            yield return new WaitUntil(() => ServiceLocator.GetService<GlobalFade>().IsDark);

            playerSystem.enabled = true;

            playerSystem.TPFDScript.enabled = true;
        }

        /// <inheritdoc/>
        public override void EndOfActivationHandler()
        {
            subAreaCollider.enabled = true;

            Data.SetActivated(false);

            inLeaveProcess.SetValue(false);

            if (!AlreadyInTPFD())
                inDialogue.Value = false;

            if (AlreadyInTPFD())
                StartCoroutine(PostDeactivateTPFD());
            else
                FinishHandler();
        }

        /// <summary>
        ///
        /// </summary>
        protected IEnumerator PostDeactivateTPFD()
        {
            yield return new WaitForSeconds(fadeTime);

            FinishHandler();

            yield return new WaitUntil(() => !inDialogue.Value);

            yield return ServiceLocator.GetService<GlobalFade>().FadeOutRoutine(fadeTime);

            UnityEngine.Debug.Log($"REPORT: {gameObject.name} Added Back TPFD Door Controls");

            GetTPFDDoors().ForEach(door => door.AddDoorControl());
        }

        #endregion
    }
}