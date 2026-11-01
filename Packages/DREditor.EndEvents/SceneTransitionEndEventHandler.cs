//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.RoomLoading;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + 
        nameof(IDialogueEndEventRoutine) + ", " + nameof(IClearDialogueSaveData) + ", " + nameof(IDRESaveable) +
        "\n\n")]
    public class SceneTransitionEndEventHandler : DialogueComponent, IDialogueEndEventRoutine, IClearDialogueSaveData, IDRESaveable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inLoading = null;

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
        [SerializeField] protected string transitionNumSaveKey = "TransitionNum";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float sceneTransitionToDarkFadeTime = 0.5f;

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
        protected int saveTransitionNum = 0;


        /// <summary>
        ///
        /// </summary>
        /// <param name="num"></param>
        public virtual void SetSaveTransitionNum(int num) => saveTransitionNum = num;


        /// <inheritdoc/>
        public virtual IEnumerator EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action exitParentEarly) 
            where T : Line
        {
            if (currentDialogue.SceneTransition.Enabled)
                yield return SceneTransitionRoutine(currentDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        protected virtual IEnumerator SceneTransitionRoutine<T>(DialogueBase<T> currentDialogue) 
            where T : Line
        {
            if (currentDialogue.SceneTransition.ToDark)
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(sceneTransitionToDarkFadeTime);

            EvaluateSceneTransitionOptions(currentDialogue);

            if (!currentDialogue.SceneTransition.AtEnd && !currentDialogue.SceneTransition.ToMenu)
                yield return MoveToScene(currentDialogue.SceneTransition.Scene);
            else if (currentDialogue.SceneTransition.AtEnd)
                DialogueEventSystem.TriggerEvent(moveOnEndEventString, currentDialogue.SceneTransition.AtEnd);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        protected virtual void EvaluateSceneTransitionOptions<T>(DialogueBase<T> currentDialogue) 
            where T : Line
        {
            if (!string.IsNullOrEmpty(currentDialogue.SceneTransition.MoveToObjective))
                moveToObjective.SetValue(currentDialogue.SceneTransition.MoveToObjective);

            if (currentDialogue.SceneTransition.MoveToNextNaturalObjective)
                moveToNextNaturalObjective.Raise();

            if (currentDialogue.SceneTransition.OnLoadNoDark)
                DialogueEventSystem.TriggerEvent(fadeOutEventString);

            if (!currentDialogue.SceneTransition.ToMenu && currentDialogue.DirectTo.Enabled)
                ServiceLocator.GetService<TPFDRoomLoader>().SetMovingToTPFDMode();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        protected virtual IEnumerator MoveToScene(string scene)
        {
            UnityEngine.Debug.Log($"{ToString()}: CHANGING SCENE");

            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(scene);

            Scene sceneToLoad = SceneManager.GetSceneByName(scene);

            asyncLoadOperation.allowSceneActivation = false;

            yield return new WaitUntil(() => asyncLoadOperation.progress >= 0.9f);

            asyncLoadOperation.allowSceneActivation = true;

            yield return new WaitUntil(() => sceneToLoad.isLoaded);

            yield return new WaitUntil(() => !inLoading.Value);
        }
        

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            if (!ServiceLocator.GetService<IDialogueManager>().GetComponentsInChildren<DialogueComponent>().Contains(this))
                return;

            saveData.Set(transitionNumSaveKey, saveTransitionNum);
        }

        /// <inheritdoc/>
        public virtual void ClearSaveData() => saveTransitionNum = 0;
    }
}