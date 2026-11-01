//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.RoomLoading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class MoveToNextDayEvent : DialogueEventHandler, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Header("")]
        [Divider]
        [Space]
        [Header("")]
        [Divider]
        [Space]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLoading = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent cleanUpDialogueReader = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent toSkipHideCG = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneTransition transition = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string moveOnEndEventString = "MoveEndOnDialogueFinish";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string fadeOutEventString = "FadeOutOnLoadSubscribe";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent moveToNextNaturalObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent moveToObjective = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentAddState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int additionalStateIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToDarkDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ReturnToMenuHandler returnToMenuHandler = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            UnityEngine.Debug.Log($"{ToString()}: MOVING TO NEXT DAY.");

            if (o is SceneTransition sentTransition)
                CallSceneTransition(sentTransition);
            else
                CallSceneTransition(transition);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneTransition"></param>
        protected virtual void CallSceneTransition(SceneTransition sceneTransition) 
            => StartCoroutine(SceneTransitionRoutine(sceneTransition));

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneTransition"></param>
        /// <returns></returns>
        protected virtual IEnumerator SceneTransitionRoutine(SceneTransition sceneTransition)
        {
            currentAddState.SetValue(additionalStateIndex);

            if (sceneTransition.ToDark)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeToDarkDuration);

            EvaluateSceneTransitionOptions(sceneTransition);

            if (!sceneTransition.AtEnd && !sceneTransition.ToMenu)
                yield return MoveToScene(sceneTransition.Scene);
            else if (sceneTransition.AtEnd)
                DialogueEventSystem.TriggerEvent(moveOnEndEventString, sceneTransition.AtEnd);

            cleanUpDialogueReader.Raise();

            if (sceneTransition.ToMenu)
            {
                returnToMenuHandler.ReturnToMenu(sceneTransition.Scene);

                yield break;
            }

            toSkipHideCG.SetValue(true);

            ServiceLocator.GetService<IDialogueManager>().GetEnder().StopDialogue();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneTransition"></param>
        protected virtual void EvaluateSceneTransitionOptions(SceneTransition sceneTransition)
        {
            if (!string.IsNullOrEmpty(sceneTransition.MoveToObjective))
                moveToObjective.SetValue(sceneTransition.MoveToObjective);

            if (sceneTransition.MoveToNextNaturalObjective)
                moveToNextNaturalObjective.Raise();

            if (sceneTransition.OnLoadNoDark)
                DialogueEventSystem.TriggerEvent(fadeOutEventString);

            if (!sceneTransition.ToMenu)
                ServiceLocator.GetService<TPFDRoomLoader>().SetMovingToTPFDMode();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        protected virtual IEnumerator MoveToScene(string scene)
        {
            UnityEngine.Debug.Log($"{ToString()}: CHANGING SCENE");

            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(scene);

            asyncLoadOperation.allowSceneActivation = false;

            yield return new WaitUntil(() => asyncLoadOperation.progress >= 0.9f);

            asyncLoadOperation.allowSceneActivation = true;

            yield return new WaitUntil(() => !inLoading.Value);

            yield break;
        }


        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("NextDay");

        protected virtual void Reset() => SetDefaultEventString("NextDay");


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => moveToObjective.SetValue(string.Empty);
    }
}