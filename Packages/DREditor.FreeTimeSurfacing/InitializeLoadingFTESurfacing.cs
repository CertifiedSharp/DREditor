//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor;
using DREditor.Dialogues;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class InitializeLoadingFTESurfacing : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent onFTEMinigame = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent observeModeLocked = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener onFinishedLoadingRoomNoDialogue = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool setControlsKey = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string controlsKey = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool endOfRoomLoad = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useFadeOut = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool ignoreWaitTime = false;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(NotIgnoringWaitTime))]
        [Tooltip("")]
        [SerializeField] protected float baseWaitTime = 2;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(NotIgnoringWaitTime))]
        [Tooltip("")]
        [SerializeField] protected float postWaitTime = 1;


        protected virtual void OnEnable()
        {
            DisableMenuObserve();

            SubscribeStartUp();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableMenuObserve()
        {
            hasMenuAccess.SetValue(false);

            observeModeLocked.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeStartUp()
        {
            if (endOfRoomLoad)
                onFinishedLoadingRoomNoDialogue.Response.AddListener(StartUp);
            else if (ServiceLocator.HasService(out IDialogueManager manager) &&
                manager.GetFinisher() is IInstantDialogueFinisherSubscriber subscriber)
                subscriber.SubscribeToOnFinishOfInstantDialogue(StartUp);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartUp()
        {
            DisableMenuObserve();

            StartCoroutine(FadeOutOnLoad());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FadeOutOnLoad()
        {
            onFTEMinigame.SetValue(true);

            if (!ignoreWaitTime)
                yield return new WaitForSeconds(baseWaitTime);

            if (useFadeOut && ServiceLocator.GetService<GlobalFade>().IsDark)
                ServiceLocator.GetService<GlobalFade>().FadeOut(postWaitTime);

            if (!ignoreWaitTime)
                yield return new WaitForSecondsRealtime(postWaitTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool NotIgnoringWaitTime() => !ignoreWaitTime;


        protected virtual void OnDisable()
        {
            UnsubscribeStartUp();

            EnableMenuObserve();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeStartUp()
        {
            if (endOfRoomLoad)
                onFinishedLoadingRoomNoDialogue.Response.RemoveListener(StartUp);
            else if (ServiceLocator.HasService<IDialogueManager>(out var manager) &&
                manager.GetFinisher() is IInstantDialogueFinisherSubscriber subscriber)
                subscriber.UnsubscribeToOnFinishOfInstantDialogue(StartUp);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableMenuObserve()
        {
            onFTEMinigame.SetValue(false);

            observeModeLocked.SetValue(false);
        }


        protected virtual void Update()
        {
            hasMenuAccess.SetValue(false);

            if (setControlsKey)
                currentControlsKey.SetValue(controlsKey);
        }
    }
}