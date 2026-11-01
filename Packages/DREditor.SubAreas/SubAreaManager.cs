//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.LocalDialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [RequireComponent(typeof(BoxCollider))]
    [ClassHeader(nameof(SubAreaManagerBase) + ", " + nameof(ISubArea) + ", " + nameof(ISelectableObject) +
        "\n\n")]
    public class SubAreaManager : SubAreaManagerBase, ISubArea
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SelectableObject focusedActor = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string displayName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SubAreaData Data = new();


        /// <summary>
        ///
        /// </summary>
        [Space(10)]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SubAreaActivationHandler activator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SubAreaActivationHandler> subAreaHandlers = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onLoadingThreeDRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedRoomTransitions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string enableMonobehavioursEvent = "EnableControlMonobehaviour";


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            onFinishedRoomTransitions.Response.AddListener(CheckLoad);

            onLoadingThreeDRoom.Response.AddListener(SetControlsFromLoadedData);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            onFinishedRoomTransitions.Response.RemoveListener(CheckLoad);

            onLoadingThreeDRoom.Response.RemoveListener(SetControlsFromLoadedData);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckLoad() => StartCoroutine(BufferCheckLoad());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferCheckLoad()
        {
            yield return null;

            if (ActivateSubAreaOnLoad())
                ActivateSubArea();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ActivateSubAreaOnLoad() => Data.activated && Data.EnterMissingOrUnlocked();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ActivateSubArea() => activator.BeginActivationState();


        /// <summary>
        ///
        /// </summary>
        protected virtual void SetControlsFromLoadedData()
        {
            if (!Data.activated)
                DialogueEventSystem.TriggerEvent(enableMonobehavioursEvent);
        }


        /// <inheritdoc/>
        public virtual void SelectObject()
        {
            if (!SubAreaLocked())
                ActivateSubArea();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SubAreaLocked() =>
            Data.EnterDataValid() &&
            (DialoguePlayedOnAccessBlocked(Data.EnteringPermanentlyLocked(), Data.leaveData.lockDialogues) ||
            DialoguePlayedOnAccessBlocked(!Data.EnteringUnlocked(), Data.enterData.lockDialogues));

        /// <summary>
        ///
        /// </summary>
        /// <param name="blocked"></param>
        /// <param name="dialogues"></param>
        /// <returns></returns>
        protected virtual bool DialoguePlayedOnAccessBlocked(bool blocked, List<LocalDialogue> dialogues)
            => blocked && TryPlayingDialogue(focusedActor, dialogues);


        /// <inheritdoc/>
        public virtual string DisplayName
        {
            get => displayName;

            set => displayName = value;
        }


        /// <inheritdoc/>
        public virtual object Save() => (SubAreaData)Data.Clone();

        /// <inheritdoc/>
        public virtual void Load(object areaData)
        {
            Data = (SubAreaData)((SubAreaData)areaData).Clone();

            if (Data.activated)
                inTPFD.Value = true;

            foreach (var handler in subAreaHandlers)
                handler.Initialize(Data);
        }


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue() => Data is IDialogueHolder holder && holder.CheckIfSaveDialogue();
    }
}