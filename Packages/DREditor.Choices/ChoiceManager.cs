//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Progressing.Handling;
using DREditor.Services;
using DREditor.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DREditor.Choices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<IChoiceManager>) + " for type: " + nameof(IChoiceManager) +
        "\n\n")]
    public class ChoiceManager : ServiceHolder<IChoiceManager>, IChoiceManager, IDialogueInteruptable, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Space]
        [Divider]
        [Space]
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLeaving = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent fastForwardModeWasEnabled = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase ChoiceGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ChoiceSaveHandler choiceSaveHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float displayChoiceBufferTime = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<ChoiceOptionUI> choiceOptions = new();


        [Header("")]
        [SerializeField] protected UnityEvent OnChoiceStarted = null;

        [Header("")]
        [SerializeField] protected UnityEvent OnChoiceConfirmed = null;


        public event Action<DialogueCore[]> OnInteruptWithNewDialogue;


        /// <summary>
        ///
        /// </summary>
        protected List<Action> allChoiceActions = new();

        /// <summary>
        ///
        /// </summary>
        protected Dialogue currentDialogue;

        /// <summary>
        ///
        /// </summary>
        protected Action exitParentRoutine = null;

        /// <summary>
        ///
        /// </summary>
        protected int selectedChoiceIndex = -1;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            ClearChoiceText();
        }

        /// <inheritdoc/>
        public virtual IEnumerator ExecuteChoiceHandling(Dialogue currentDialogue, Action exitParentRoutine = null)
        {
            this.currentDialogue = currentDialogue;

            this.exitParentRoutine = exitParentRoutine;

            InitializeChoices();

            if (ChoiceGroup != null)
            {
                if (displayChoiceBufferTime > 0)
                    yield return new WaitForSeconds(displayChoiceBufferTime);

                ChoiceGroup.Show();

                yield return ChoiceGroup.WaitForChangeToComplete();
            }

            OnChoiceStarted?.Invoke();

            EventSystem.current.SetSelectedGameObject(choiceOptions[0].gameObject);

            yield return WaitForPlayerClick();

            OnChoiceConfirmed?.Invoke();

            EvaluateSelectedChoice();

            EventSystem.current.SetSelectedGameObject(null);

            if (ChoiceGroup != null)
            {
                ChoiceGroup.Hide();

                yield return ChoiceGroup.WaitForChangeToComplete();
            }

            PlayChoiceDialogue();

            TriggerCustomChoiceDialogueEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeChoices()
        {
            for (int i = 0; i < currentDialogue.Choices.Count; i++)
            {
                if (currentDialogue.Choices[i].ChoiceText != "")
                {
                    choiceOptions[i].gameObject.SetActive(true);

                    choiceOptions[i].SetText(currentDialogue.Choices[i].ChoiceText);
                }
                else
                    choiceOptions[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual IEnumerator WaitForPlayerClick()
        {
            GameObject lastSelectedGO = null;

            while (!submit.action.triggered)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(lastSelectedGO);
                else
                    lastSelectedGO = EventSystem.current.currentSelectedGameObject;

                if (submit.action.triggered)
                    break;

                yield return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateSelectedChoice()
        {
            var choice = EventSystem.current.currentSelectedGameObject;

            for (int i = 0; i < choiceOptions.Count; i++)
                if (choice == choiceOptions[i].gameObject)
                    choiceOptions[i].InvokeChoice();

            selectedChoiceIndex = choiceOptions.IndexOf(choice.GetComponent<ChoiceOptionUI>());

            UnityEngine.Debug.Log(ToString() + ": Choice Chosen Index was: " + selectedChoiceIndex);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PlayChoiceDialogue()
        {
            if (!isLeaving.Value && currentDialogue.HasChoiceDialogueAtIndex(selectedChoiceIndex))
            {
                TriggerDialogueFlag();

                fastForwardModeWasEnabled.SetValue(true);

                Dialogue[] dialogues = { currentDialogue.Choices[selectedChoiceIndex].NextDialogue };

                choiceSaveHandler.AddChoice(selectedChoiceIndex);

                exitParentRoutine?.Invoke();

                DialogueInterruptedInvoke(dialogues);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TriggerCustomChoiceDialogueEvent()
        {
            if (!isLeaving.Value && currentDialogue.Choices[selectedChoiceIndex].CustomCall != "")
            {
                TriggerDialogueFlag();

                DialogueEventSystem.TriggerEvent(currentDialogue.Choices[selectedChoiceIndex].CustomCall);

                if (currentDialogue.Choices[selectedChoiceIndex].CallBreak)
                    exitParentRoutine?.Invoke();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TriggerDialogueFlag()
        {
            if (currentDialogue.FlagTrigger.Enabled)
                ServiceLocator.GetService<FlagTriggerHandlerBase>().TriggerFlag(
                    currentDialogue.FlagTrigger.Chapter,
                    currentDialogue.FlagTrigger.Objective,
                    currentDialogue.FlagTrigger.Flags);
        }


        /// <inheritdoc/>
        public virtual void SubscribeAllChoices(Action[] actionArray)
        {
            for (int i = 0; i < actionArray.Length; i++)
                SubscribeChoice(i, actionArray[i]);
        }

        /// <inheritdoc/>
        public virtual void SubscribeChoice(int index, Action action)
        {
            choiceOptions[index].SubscribeChoice(action);

            allChoiceActions.Add(action);
        }

        /// <inheritdoc/>
        public virtual void UnsubscribeChoice(int index, Action action)
        {
            choiceOptions[index].UnsubscribeChoice(action);

            allChoiceActions.Remove(action);
        }

        /// <inheritdoc/>
        public virtual void UnsubscribeAllChoices()
        {
            for (int i = 0; i < choiceOptions.Count; i++)
                for (int x = 0; x < allChoiceActions.Count; x++)
                    UnsubscribeChoice(i, allChoiceActions[x]);
        }

        /// <inheritdoc/>
        public virtual void ClearChoiceText()
        {
            foreach (var choice in choiceOptions)
                if (choice != null)
                    choice.Clear();
        }


        /// <inheritdoc/>
        public virtual void DialogueInterruptedInvoke(DialogueCore[] dialogues)
        {
            if (OnInteruptWithNewDialogue == null)
                UnityEngine.Debug.LogWarning("ALERT: Nothing has subscribed to intercept the chosen choices dialogue!" +
                    $"\nSomething must subscribe to this {nameof(IDialogueInteruptable)}'s {nameof(OnInteruptWithNewDialogue)} event!");

            OnInteruptWithNewDialogue?.Invoke(dialogues);
        }

        //Testing /// <inheritdoc/>
        /// <inheritdoc/>
        public virtual void ToTitleScreen() => isLeaving.Value = false;
    }
}