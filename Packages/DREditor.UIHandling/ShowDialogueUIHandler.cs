//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueUIComponent) + ", " + nameof(DialogueComponent) + ", " + nameof(IPreparableDialogueComponent) +
        "\n\n")]
    public class ShowDialogueUIHandler : DialogueUIComponent, IPreparableDialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float prepareOrder = 4;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int preparerID = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool enableCamerasOnShow = true;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(EnableCamerasOnShow))]
        [Tooltip("")]
        [SerializeField] protected List<string> enableCameraStringEvents = new()
        {
            "EnableDialogueCamera", "EnableBlurCamera"
        };


        [Header("")]
        [SerializeField] protected UnityEvent OnShow;


        public float Order { get => prepareOrder; }

        public int ExclusiveHandlerID => preparerID;


        /// <inheritdoc/>
        public virtual bool UseRoutineToPrepare() => !inTrialMode.Value;

        /// <inheritdoc/>
        public virtual void Prepare() => ShowDialogueUI();

        /// <inheritdoc/>
        public virtual IEnumerator PrepareRoutine()
        {
            Prepare();

            yield return WaitForVisibilityChange();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowDialogueUI()
        {
            EnableCameraEvents();

            SetDialogueUIVisibility();

            EvaluateUISFX();

            OnShow?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableCameraEvents()
        {
            if (EnableCamerasOnShow())
                DialogueEventSystem.TriggerEvents(enableCameraStringEvents);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool EnableCamerasOnShow() => enableCamerasOnShow;

        /// <inheritdoc/>
        protected override void SetDialogueUIVisibility() => visibleUI?.Show();
    }
}