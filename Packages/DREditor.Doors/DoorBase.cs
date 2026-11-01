//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Choices;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.LocalDialogues;
using DREditor.Processables;
using DREditor.RoomLoading;
using DREditor.Services;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class DoorBase : MonoBehaviour, IGateHolder, INameDisplayable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Gate setGateway = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string tpfdControlsKey = "tpfd";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool isHitbox = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool isTPFD = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DoorData data = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string disableAbilityEvent = "DisableAbility";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLeaveProcess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent checkOverlay = null;


        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager DialogueManager;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (ServiceLocator.HasService<IDialogueManager>())
                DialogueManager = ServiceLocator.GetService<IDialogueManager>();
        }


        #region Check Locks

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckLocks()
        {
            if (DoorIsLocked())
                EvaluateLock();
            else
                PassedLock();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DoorIsLocked() => data.lockData.IsLocked();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateLock()
        {
            UnityEngine.Debug.Log($"Door {gameObject.name} is checking for locked dialogue.");

            Dialogue dialogue = ServiceLocator.GetService<LocalDialogueHandlerBase>().EvaluateDialogue(data.lockData.lockDialogues);

            if (DialogueIsValid(dialogue))
                DoorLocked(dialogue);
            else
                PassedLock();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual bool DialogueIsValid(Dialogue dialogue) => dialogue != null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        protected virtual void DoorLocked(Dialogue dialogue)
        {
            hasMenuAccess.SetValue(false);

            DialogueManager.StartDialogue(null, dialogue);

            inLeaveProcess.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PassedLock()
        {
            if (isTPFD)
                PassedLockTPFD();
            else
                PassedLockThreeD();
        }

        #endregion

        #region PassedLockTPFD

        /// <summary>
        ///
        /// </summary>
        protected virtual void PassedLockTPFD()
        {
            UnityEngine.Debug.Log($"REPORT: Door {gameObject.name}: TPFD Room Unlocked");

            hasMenuAccess.SetValue(false);

            if (ServiceLocator.GetService<LeaveAskManager>().GetLeaveAskDialogue() == null)
                UnityEngine.Debug.LogError($"ALERT: Leave Ask has not been added! Please add it to the {nameof(LeaveAskManager)}!");

            Dialogue dialogue = (Dialogue)ServiceLocator.GetService<LeaveAskManager>().GetLeaveAskDialogue();

            ServiceLocator.GetService<IChoiceManager>().SubscribeAllChoices(new Action[2] { ChangeRoom, DummyNoOption });

            DialogueManager.StartDialogue(null, dialogue, true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DummyNoOption()
        {
            ServiceLocator.GetService<IChoiceManager>().UnsubscribeAllChoices();

            EventSystem.current.SetSelectedGameObject(null);

            currentControlsKey.SetValue(tpfdControlsKey);

            inLeaveProcess.SetValue(false);
        }

        #endregion

        #region PassedLockThreeD

        /// <summary>
        ///
        /// </summary>
        protected virtual void PassedLockThreeD()
        {
            if (data.lockData.unlockedDialogues.Count > 0)
                EvaluateUnlockDialogue();
            else
                ChangeRoom();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateUnlockDialogue()
        {
            Dialogue dialogue = ServiceLocator.GetService<LocalDialogueHandlerBase>().EvaluateDialogue(data.lockData.unlockedDialogues);

            if (DialogueIsValid(dialogue))
                PlayUnlockedDialogue(dialogue);
            else
                ChangeRoom();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        protected virtual void PlayUnlockedDialogue(Dialogue dialogue)
        {
            DialogueManager.StartDialogue(null, dialogue);

            var endDialogueEvents = FindObjectsByType<MonoBehaviour>().OfType<IEndDialogueEvent>();
            
            foreach(var endEvent in endDialogueEvents)
                endEvent.OnDialogueEnd += ChangeRoom;
        }

        #endregion

        #region ChangeRoom

        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeRoom()
        {
            ServiceLocator.GetService<IChoiceManager>().UnsubscribeAllChoices();

            var endDialogueEvents = FindObjectsByType<MonoBehaviour>().OfType<IEndDialogueEvent>();

            foreach (var endEvent in endDialogueEvents)
                endEvent.OnDialogueEnd -= ChangeRoom;

            if (gameObject == null)
            {
                UnityEngine.Debug.LogWarning($"ALERT: The door trying to change the room does not exist!");

                return;
            }

            UnityEngine.Debug.Log($"REPORT: Entering door {gameObject.name} to change room.");

            checkOverlay.Raise();

            SetIdentified(true);

            DialogueEventSystem.TriggerEvent(disableAbilityEvent);

            FindAnyObjectByType<PnCCamera>().Controllable = false;

            FindAnyObjectByType<SmoothMouseLook>().enabled = false;

            ServiceLocator.GetService<RoomChangerBase>().ChangeRoom(setGateway, data.roomLoadOptions);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="identified"></param>
        protected virtual void SetIdentified(bool identified) => data.SetIdentified(identified);

        #endregion


        /// <inheritdoc/>
        public virtual string DisplayName
        {
            get => data.identified ? data.displayName
            : data.unidentifiedDisplayName == "" ? "???" : data.unidentifiedDisplayName;

            set => data.SetDisplayName(value);
        }

        /// <inheritdoc/>
        public virtual Gate GetSetGateway() => setGateway;
    }
}