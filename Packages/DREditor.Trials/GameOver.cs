//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.RoomLoading;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(GameOverBase) +
        "\n\n")]
    public class GameOver : GameOverBase
    {
#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;
#endif
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string gameOverContinueEvent = "OnGameOverContinue";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialContinued = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent playerHasDied = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ReturnToMenuHandler menuHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent gameOverVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Button continueButton = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Button giveUpButton = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Button> additionalButtons = new();


        [Header("")]
        [SerializeField] protected UnityEvent OnGameOver;


        protected virtual void Awake()
        {
            InitializeButtons();

            SetButtonInteractability(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeButtons()
        {
            continueButton.onClick.AddListener(ContinueGame);

            giveUpButton.onClick.AddListener(GiveUp);

            additionalButtons.Add(continueButton);

            additionalButtons.Add(giveUpButton);
        }

#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Debug Game Over")]
        public void DebugGameOver() => Start();

        protected virtual void Start()
        {
            if (debugMode)
                StartCoroutine(BufferGameOverDebug());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferGameOverDebug()
        {
            yield return new WaitForSeconds(2);

            GameIsOver();
        }

#endif
        

        /// <inheritdoc/>
        protected override void SubscribeToPlayerDeath()
        {
            playerHasDied.AddListener(OnPlayerStateUpdated);

#if UNITY_EDITOR
            if (debugMode)
                return;
#endif
            if (gameOverVisibility != null)
                gameOverVisibility.Hide();
        }

        /// <inheritdoc/>
        protected override void UnsubscribeToPlayerDeath() => playerHasDied.RemoveListener(OnPlayerStateUpdated);

        /// <summary>
        ///
        /// </summary>
        /// <param name="playerHasDied"></param>
        protected virtual void OnPlayerStateUpdated(bool playerHasDied)
        {
            if (playerHasDied)
                GameIsOver();
        }

        /// <inheritdoc/>
        protected override void GameIsOver()
        {
            SetButtonInteractability(true);

            OnGameOver?.Invoke();

            if (gameOverVisibility != null)
                StartCoroutine(ShowGameOverRoutine());
            else
                EventSystem.current.SetSelectedGameObject(additionalButtons[0].gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowGameOverRoutine()
        {
            yield return gameOverVisibility.ShowTillComplete();

            EventSystem.current.SetSelectedGameObject(additionalButtons[0].gameObject);
        }


        /// <inheritdoc/>
        public override void GiveUp()
        {
            SetButtonInteractability(false);

            ServiceLocator.GetService<RoomLoaderBase>().DoNotAllowRoomsToLoad();

            var monos = FindObjectsByType<MonoBehaviour>();

            monos.OfType<DialogueReader>().ToList().ForEach(reader => reader.StopAllCoroutines());

            menuHandler.ReturnToMenu();
        }

        /// <inheritdoc/>
        public override void ContinueGame()
        {
            SetButtonInteractability(false);

            EventSystem.current.SetSelectedGameObject(null);

            if (DialogueEventSystem.IsEventRegistered(gameOverContinueEvent))
                DialogueEventSystem.TriggerEvent(gameOverContinueEvent);
            else
                trialContinued.SetValue(true);

            if (gameOverVisibility != null)
                gameOverVisibility.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interactable"></param>
        protected virtual void SetButtonInteractability(bool interactable) 
            => additionalButtons.ForEach(button => button.interactable = interactable);
    }
}