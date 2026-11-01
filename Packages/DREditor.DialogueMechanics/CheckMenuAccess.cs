//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineWaitForClick) +
        "\n\n")]
    public class CheckMenuAccess : DialogueComponent, ILineWaitForClick
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDebate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialMistake = null;


        public virtual bool SkipOnFastForward => true;


        /// <inheritdoc/>
        public IEnumerator LineWaitForClick<T>(DialogueBase<T> currentDialogue, Line currentLine, Action exitParentCallback = null) 
            where T : Line
        {
            if ((inDebate == null || !inDebate.Value) && (trialMistake == null || !trialMistake.Value))
                EvaluateMenuAccess(currentDialogue, currentLine);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="currentLine"></param>
        protected virtual void EvaluateMenuAccess<T>(DialogueBase<T> currentDialogue, Line currentLine)
            where T : Line
        {
            bool OnLastLine = currentDialogue.Lines.IndexOf((T)currentLine) == currentDialogue.Lines.Count - 1;

            bool hasChoices = OnLastLine && currentDialogue is Dialogue dialogue && dialogue.Choices.Count != 0;

            bool notFastForwardInRegularDialogue = !inInstant.Value && !inFastForwardMode.Value;

            if (!hasChoices && (inInstant.Value || notFastForwardInRegularDialogue))
                hasMenuAccess.SetValue(true);
        }
    }
}