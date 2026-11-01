//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TrialPrepUIBase : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string trialSceneName = "GYM_Trial";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string disablePlayerEvent = "DisablePlayer";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent startPauseGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedLoadingRoom = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent skipActivateControls = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent controlsMenuForceDisable = null;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (inDialogue.Value)
                inDialogue.AddListener(OnDialogueStateChanged);

            Response?.AddListener(OnResumeFinished);

            controlsMenuForceDisable.SetValue(true);

            SubscribeSkipControls();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inDialogue"></param>
        protected virtual void OnDialogueStateChanged(bool inDialogue)
        {
            this.inDialogue.RemoveListener(OnDialogueStateChanged);

            if (!inDialogue)
                Initialize();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeSkipControls()
        {
            if (!isLoadingFile.Value)
            {
                skipActivateControls.SetValue(true);

                onFinishedLoadingRoom.Response.AddListener(UnsubscribeSkipControls);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeSkipControls()
        {
            onFinishedLoadingRoom.Response.RemoveListener(UnsubscribeSkipControls);

            skipActivateControls.SetValue(false);
        }


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            inDialogue.RemoveListener(OnDialogueStateChanged);

            Response?.RemoveListener(OnResumeFinished);

            controlsMenuForceDisable.SetValue(false);

            UnsubscribeSkipControls();
        }


        protected virtual void Start()
        {
            Initialize();

            StartCoroutine(InitializePreparation());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            hasMenuAccess.SetValue(false);

            ServiceLocator.GetService<MusicManagerBase>().StopPlayingMusic();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void InitializeTrialState()
        {
            if (!inTrialMode.Value)
                inTrialMode.SetValue(true);

            DialogueEventSystem.TriggerEvent(disablePlayerEvent);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator InitializePreparation();

        /// <summary>
        ///
        /// </summary>
        public abstract void ShowPause();

        /// <summary>
        ///
        /// </summary>
        public abstract void OnResumeFinished();

        /// <summary>
        ///
        /// </summary>
        public abstract void BeginTrial();
    }
}