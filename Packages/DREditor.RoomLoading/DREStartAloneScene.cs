//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDialogueHolder) +
        "\n\n")]
    public class DREStartAloneScene : MonoBehaviour, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Dialogue startingDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent startingMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent observeModeLocked = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent reticleSelectOverride = null;

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
        [SerializeField] protected float startSceneDialogueBufferTime = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObject actor = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => InitializeGameStates();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeGameStates()
        {
            reticleSelectOverride.SetValue(false);

            observeModeLocked.SetValue(true);

            SetGameToDesignatedMode();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetGameToDesignatedMode() => startingMode.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => StartSceneProcessFromLoadSituation();

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartSceneProcessFromLoadSituation()
        {
            if (isLoadingFile.Value)
                StartCoroutine(AssertObserveModeUnlockAfterDialogue());
            else
                SubscribeToSceneEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToSceneEvent()
            => onFinishedLoadingRoomNoDialogue.Response.AddListener(StartGameSceneDialogue);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator AssertObserveModeUnlockAfterDialogue()
        {
            SetGameToDesignatedMode();

            yield return new WaitUntil(() => inDialogue.Value);

            observeModeLocked.SetValue(false);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartGameSceneDialogue() => StartCoroutine(StartGameSceneDialogueRoutine());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator StartGameSceneDialogueRoutine()
        {
            yield return new WaitForSeconds(startSceneDialogueBufferTime);

            inDialogue.SetValue(true);

            observeModeLocked.SetValue(false);

            ServiceLocator.GetService<IDialogueManager>().StartDialogue(actor, new Dialogue[1] { startingDialogue });

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => UnsubscribeToSceneEvent();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToSceneEvent() 
            => onFinishedLoadingRoomNoDialogue.Response.RemoveListener(StartGameSceneDialogue);

        
        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
        {
            return ServiceLocator.GetService<IDialogueManager>()
                .GetManagerComponent<DialogueLoadIdentifier>()
                .CheckDialogue<Line>(startingDialogue);
        }
    }
}