//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineWaitForClick) +
        "\n\n")]
    public class WaitForDialogueClick : DialogueComponent, ILineWaitForClick
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference submit = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isUIToggled = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineWaitForClickComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UnityEvent OnDialogueClicked;


        public virtual bool SkipOnFastForward => true;


        /// <inheritdoc/>
        public virtual IEnumerator LineWaitForClick<T>(DialogueBase<T> currentDialogue, Line currentLine,
            Action exitParentCallback = null)
            where T : Line
        {
            if (currentDialogue is Dialogue dialogue)
            {
                bool OnLastLine = dialogue.Lines.IndexOf(currentLine) == currentDialogue.Lines.Count - 1;

                bool hasChoices = OnLastLine && dialogue.Choices.Count != 0;

                if (!hasChoices)
                    yield return WaitForButtonClick(currentDialogue, currentLine);
            }
            else
                yield return WaitForButtonClick(currentDialogue, currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual IEnumerator WaitForButtonClick<T>(DialogueBase<T> currentDialogue, Line currentLine)
            where T : Line
        {
            yield return WaitForPlayerClick(currentDialogue, currentLine);

            yield return WaitOutOfToggleMenu();

            if (!inFastForwardMode.Value)
                OnDialogueClicked?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator WaitForPlayerClick<T>(DialogueBase<T> currentDialogue, Line currentLine)
            where T : Line
        {
            while (!submit.action.triggered)
            {
                yield return WaitOutOfToggleMenu();

                bool exitParent = false;

                yield return InvokeWaitForClickComponents(currentDialogue, currentLine, () => { exitParent = true; });

                if (exitParent)
                    yield break;

                // Break out once the player clicks or we're in fast forward
                if (submit.action.triggered || inFastForwardMode.Value)
                    break;

                yield return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitOutOfToggleMenu()
        {
            while (!isUIToggled.Value)
                yield return null;

            while (inMenu.Value)
                yield return null;

            while (ServiceLocator.GetService<GlobalFade>().IsDark || ServiceLocator.GetService<GlobalFade>().isTransitioning)
                yield return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="currentLine"></param>
        /// <param name="exitParentRoutine"></param>
        /// <remarks></remarks>
        protected virtual IEnumerator InvokeWaitForClickComponents
            <T>(DialogueBase<T> currentDialogue, Line currentLine, Action exitParentRoutine) 
             where T : Line
        {
            foreach (ILineWaitForClick waitForClickProcessor in LineWaitForClickComponents.Cast<ILineWaitForClick>())
            {
                bool exitParent = false;

                if (submit.action.triggered)
                {
                    exitParentRoutine?.Invoke();

                    yield break;
                }

                yield return waitForClickProcessor.LineWaitForClick(currentDialogue, currentLine, () => { exitParent = true; });

                if (exitParent || submit.action.triggered)
                {
                    exitParentRoutine?.Invoke();

                    yield break;
                }
            }
        }
    }
}