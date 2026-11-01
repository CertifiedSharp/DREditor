//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.MC
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MinigameManagerBase) + ", " +
        "\n\n")]
    public class MultipleChoiceManager : MinigameManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MultipleChoiceBuilder builder = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TMP_Text questionText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform choicesParent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject choicePrefab = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent gameUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent introUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent answerUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float menuFadeWaitTime = 0.75f;


        [Header("")]
        [SerializeField] protected UnityEvent OnMinigameStarted;


        /// <summary>
        ///
        /// </summary>
        protected bool inSelect = false;

        /// <summary>
        ///
        /// </summary>
        protected List<Selectable> choices = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="inSelect"></param>
        protected virtual void SetInSelect(bool inSelect) => this.inSelect = inSelect;


        /// <inheritdoc/>
        protected override void OnMinigameDisable()
        {
            base.OnMinigameDisable();

            submit.action.started -= SubmitChoice;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (debugMode)
                StartChoosing(builder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        public void PlayMultipleChoice(ScriptableObject asset)
            => FindAnyObjectByType<MultipleChoiceManager>().StartChoosing((MultipleChoiceBuilder)asset);

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        public virtual void StartChoosing(MultipleChoiceBuilder builder) => StartCoroutine(BeginChoice(builder));

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BeginChoice(MultipleChoiceBuilder builder)
        {
            this.builder = builder;

            Initialize();

            if (questionText != null)
                questionText.text = builder.question;

            OnMinigameStarted?.Invoke();

            if (introUI != null)
                yield return introUI.ShowTillComplete();

            if (TrialTimer != null)
                TrialTimer.ShowInitialize(builder.times, false);

            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);

            submit.action.started += SubmitChoice;

            SetInSelect(true);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Initialize()
        {
            var choiceTexts = builder.GetChoiceTexts();

            foreach (var choiceText in choiceTexts)
            {
                if (string.IsNullOrEmpty(choiceText))
                    continue;

                GameObject choice = Instantiate(choicePrefab, choicesParent);

                choice.GetComponentInChildren<TMP_Text>().text = choiceText;

                choices.Add(choice.GetComponent<Selectable>());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SubmitChoice(CallbackContext context)
        {
            if (inSelect)
            {
                string selectedText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;

                MultipleChoiceOption selectedOption = builder.GetOptionWithText(selectedText);

                if (selectedOption == null)
                    return;

                ServiceLocator.GetService<SFXManagerBase>().PlaySubmit();

                SetInSelect(false);

                submit.action.started -= SubmitChoice;

                EventSystem.current.SetSelectedGameObject(null);

                if (TrialTimer != null)
                    TrialTimer.PauseTimer();

                HideVisuals();

                if (selectedOption.isAnswer)
                    StartCoroutine(ResolveMinigame());
                else
                    TrialMistake(selectedOption.wrongDialogue);
            }
        }

        /// <inheritdoc/>
        public override IEnumerator ResolveMinigame()
        {
            yield return new WaitForSeconds(menuFadeWaitTime);

            if (answerUI != null)
                yield return answerUI.ShowTillComplete();

            yield return base.ResolveMinigame();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideVisuals()
        {
            if (TrialTimer != null)
                TrialTimer.Hide();

            gameUI.Hide();
        }

        /// <inheritdoc/>
        protected override void Restart()
        {
            gameUI.Show();

            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);

            if (TrialTimer != null)
                TrialTimer.Show();

            StartCoroutine(ResolveRestart());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveRestart()
        {
            yield return TrialTimer.WaitForChangeToComplete();

            if (TrialTimer != null)
                TrialTimer.ResumeTimer();

            yield return new WaitForSeconds(Time.deltaTime);

            submit.action.started += SubmitChoice;

            SetInSelect(true);

            yield break;
        }


        /// <inheritdoc/>
        protected override void OnTimeUp()
        {
            SetInSelect(false);

            EventSystem.current.SetSelectedGameObject(null);

            HideVisuals();

            base.OnTimeUp();
        }
    }
}