//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Input;
using DREditor.LocalDialogues;
using DREditor.RoomLoading;
using DREditor.Services;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DoorBase) + ", " + nameof(IDoor) + ", " + nameof(IDialogueHolder) + ", " + nameof(ISelectableObject) + ", " +
        nameof(ISceneEventListener) +
        "\n\n")]
    public class Door : DoorBase, IDoor, IDialogueHolder, ISelectableObject, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent changingObserve = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingToRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string leaveAskEvent = "OnLeaveAsk";


        /// <summary>
        ///
        /// </summary>
        protected override void Start()
        {
            if (isTPFD)
                onFinishedLoadingRoom.RegisterListener(this);

            base.Start();
        }


        /// <inheritdoc/>
        public virtual void OnEventRaised() => AddDoorControl();

        /// <inheritdoc/>
        public virtual void AddDoorControl()
        {
            onFinishedLoadingRoom.UnregisterListener(this);

            ServiceLocator.GetService<IInputManager>().GetLeave().performed += EvaluateLock;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => RemoveDoorControl();

        /// <inheritdoc/>
        public virtual void RemoveDoorControl()
        {
            if (ServiceLocator.HasService<IInputManager>())
                ServiceLocator.GetService<IInputManager>().GetLeave().performed -= EvaluateLock;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateLock(CallbackContext context)
        {
            if (!inLeaveProcess.Value && !changingObserve.Value)
                LockCheck();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (isHitbox && !inLeaveProcess.Value && !isLoadingToRoom.Value)
                LockCheck();
        }

        /// <inheritdoc/>
        public virtual void SelectObject() => LockCheck();

        /// <summary>
        ///
        /// </summary>
        protected virtual void LockCheck()
        {
            if (inMenu.Value || inDialogue.Value)
                return;

            inLeaveProcess.SetValue(true);

            if (isTPFD)
                DialogueEventSystem.TriggerEvent(leaveAskEvent);

            CheckLocks();
        }


        /// <inheritdoc/>
        public virtual void ClearLockData()
        {
            SetIdentified(true);

            data.ClearLockData();
        }

        /// <inheritdoc/>
        public virtual string GetGameObjectName() => gameObject.name;

        /// <inheritdoc/>
        public virtual bool IsTPFD() => isTPFD;

        /// <inheritdoc/>
        public virtual string GetDoorName() => ((DoorData)Save()).doorName;

        /// <inheritdoc/>
        public virtual string GetSaveDisplayName() => ((DoorData)Save()).displayName;

        /// <inheritdoc/>
        public virtual object Save()
        {
            data.SetName(gameObject.name);

            return (DoorData)data.Clone();
        }

        /// <inheritdoc/>
        public virtual void Load(object data)
        {
            this.data = (DoorData)data;

            SetIdentified(this.data.identified);

            CheckIfBeenThroughDoor();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void CheckIfBeenThroughDoor()
        {
            if (Application.isPlaying && setGateway != null)
            {
                bool beenThroughDoor = ServiceLocator.GetService<InstanceManagerBase>().ContainsRoomDataFor(setGateway.ToAreaName);

                if (beenThroughDoor && !data.identified)
                    SetIdentified(beenThroughDoor);
            }
        }


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue() 
            => data.lockData.lockDialogues.CheckIfSaveDialogue()
            || data.lockData.unlockedDialogues.CheckIfSaveDialogue();
    }
}