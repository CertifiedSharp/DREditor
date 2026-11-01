//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CGHandler : CGHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string showCGStateEventString = "ShowCG";//"ShowVideo"

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideCGStateEventString = "HideCG";//"HideVideo"

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent allNameplatesTransparent = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string showMainUIEventString = "ShowMainUI";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideMainUIEventString = "HideMainUI";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float hideNonCGUIWaitTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string stopBlurVisionEventString = "StopBlur";


        /// <summary>
        ///
        /// </summary>
        protected bool endState;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            DialogueEventSystem.StartListening(showCGStateEventString, StartCGEvent);

            DialogueEventSystem.StartListening(hideCGStateEventString, EndCGEvent);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            DialogueEventSystem.StopListening(showCGStateEventString, StartCGEvent);

            DialogueEventSystem.StopListening(hideCGStateEventString, EndCGEvent);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void StartCGEvent(object o = null) => StartCoroutine(PlayCGEvent(o, false));

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void EndCGEvent(object o = null) => StartCoroutine(PlayCGEvent(o, true));

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <param name="endState"></param>
        /// <returns></returns>
        protected abstract IEnumerator PlayCGEvent(object o, bool endState);


        /// <summary>
        ///
        /// </summary>
        protected virtual void SetNameplatesTransparent()
        {
            if (allNameplatesTransparent != null)
                allNameplatesTransparent.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveCGEvent()
        {
            if (stopBlurVisionEventString != "")
                DialogueEventSystem.TriggerEvent(stopBlurVisionEventString);

            if (endState && !isGoingToMainMenu.Value && !inTrialMode.Value)
                DialogueEventSystem.TriggerEvent(showMainUIEventString);

            cgStateEnded.SetValue(true);
        }
    }
}