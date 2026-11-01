//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Finishing;
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using DREditor.UIHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(UIVisibilityHandler) +
        "\n\n")]
    public class CameraArrowManager : UIVisibilityHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<SceneEventListener> showListeners = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<SceneEventListener> hideListeners = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<string> showEvents = new() { "SubAreaOnDidntLeave" };

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> hideEvents = new() { "OnLeaveAsk" };


        /// <summary>
        ///
        /// </summary>
        protected bool listening = false;

        /// <summary>
        ///
        /// </summary>
        protected bool dialogueIsEnding = false;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IDialoguePrepareEvent> dialoguePrepareEvents;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IEndDialogueEvent> dialogueEndEvents;



        /// <summary>
        ///
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            dialoguePrepareEvents = FindObjectsByType<MonoBehaviour>().OfType<IDialoguePrepareEvent>();

            dialogueEndEvents = FindObjectsByType<MonoBehaviour>().OfType<IEndDialogueEvent>();

            OnTPFDModeChanged(inTPFD.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inTPFD"></param>
        protected virtual void OnTPFDModeChanged(bool inTPFD)
        {
            if (inTPFD && listening)
                return;

            if (inTPFD)
                StartListeningToEvents();
            else
                StopListeningToEvents();

            listening = inTPFD;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartListeningToEvents()
        {
            if (dialogueIsEnding || !inDialogue.Value)
                visibleUI.Show();

            foreach (var sceneEvent in showListeners)
                sceneEvent.AddListener(visibleUI.Show);

            foreach (var sceneEvent in hideListeners)
                sceneEvent.AddListener(visibleUI.Hide);

            DialogueEventSystem.StartListening(showEvents, ShowEvent);

            DialogueEventSystem.StartListening(hideEvents, HideEvent);

            SubscribeToDialogueEnding(true, visibleUI.Show);

            foreach (var preparable in dialoguePrepareEvents)
                preparable.OnDialoguePrepare += visibleUI.Hide;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopListeningToEvents()
        {
            if (gameObject.activeSelf)
                visibleUI.Hide();

            foreach (var sceneEvent in showListeners)
                sceneEvent.RemoveListener(visibleUI.Show);

            foreach (var sceneEvent in hideListeners)
                sceneEvent.RemoveListener(visibleUI.Hide);

            DialogueEventSystem.StopListening(showEvents, ShowEvent);

            DialogueEventSystem.StopListening(hideEvents, HideEvent);

            SubscribeToDialogueEnding(false, visibleUI.Show);

            foreach (var preparable in dialoguePrepareEvents)
                preparable.OnDialoguePrepare -= visibleUI.Hide;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void ShowEvent(object o = null) => visibleUI.Show();

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void HideEvent(object o = null) => visibleUI.Hide();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            inTPFD.AddListener(OnTPFDModeChanged);

            SubscribeToDialogueEnding(true, NoticedDialogueEnding);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            inTPFD.RemoveListener(OnTPFDModeChanged);

            SubscribeToDialogueEnding(false, NoticedDialogueEnding);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="response"></param>
        protected virtual void SubscribeToDialogueEnding(bool subscribe, Action response)
        {
            foreach (var endable in dialogueEndEvents)
                if (subscribe)
                    endable.OnDialogueEnd += response;
                else
                    endable.OnDialogueEnd -= response;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void NoticedDialogueEnding()
        {
            if (inDialogue.Value)
            {
                dialogueIsEnding = true;

                StartCoroutine(WaitForDialogueOff());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForDialogueOff()
        {
            yield return new WaitUntil(() => !inDialogue.Value);

            SubscribeToDialogueEnding(false, NoticedDialogueEnding);

            dialogueIsEnding = false;
        }


        protected virtual void OnDestroy() => StopListeningToEvents();
    }
}