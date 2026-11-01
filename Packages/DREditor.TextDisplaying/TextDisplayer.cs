//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Input;
using DREditor.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TextDisplayerBase) + ", " + nameof(IToTitleScreen) + ", " + nameof(ILineRoutine) +
        "\n\n")]
    public class TextDisplayer : TextDisplayerBase
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
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bufferTextMultiplier = 0.01f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<TextDisplayModifier> textModifiers = new();


        /// <summary>
        ///
        /// </summary>
        protected bool skipText = false;

        /// <summary>
        ///
        /// </summary>
        protected bool skipTextStarted = false;

        
        /// <inheritdoc/>
        public override void DisplayText(TMP_Text textBox, Line line)
        {
            if (inFastForwardMode.Value)
            {
                InitializeTextBox(textBox);

                string preparedText = ApplyTextModifications(line);

                PrepareTextBox(preparedText);

                if (TextBoxIsEmpty(line))
                    return;

                ResolveDisplayingText(preparedText);
            }
            else
                StartCoroutine(DisplayTextRoutine(textBox, line));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual IEnumerator DisplayTextRoutine(TMP_Text textBox, Line currentLine)
        {
            InitializeTextBox(textBox);

            string preparedText = ApplyTextModifications(currentLine);

            PrepareTextBox(preparedText);

            if (TextBoxIsEmpty(currentLine))
                yield break;

            yield return DisplayTextAnimation(preparedText);

            ResolveDisplayingText(preparedText);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textBox"></param>
        protected virtual void InitializeTextBox(TMP_Text textBox)
        {
            box = textBox;

            skipText = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="preparedText"></param>
        protected virtual void PrepareTextBox(string preparedText)
        {
            box.maxVisibleCharacters = 0;

            box.text = preparedText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual bool TextBoxIsEmpty(Line currentLine)
        {
            bool result = string.IsNullOrEmpty(box.text) || SkipDisplayingLine(currentLine);

            if (result)
            {
                SetWaitingForClick(false);

                ServiceLocator.GetService<IInputManager>().Disable();

                inFastForwardMode.SetValue(true);

                ServiceLocator.GetService<IDialogueManager>()
                .GetComponentsInChildren<MonoBehaviour>()
                .OfType<ILineReader>()
                .FirstOrDefault().OnLineEndEvent += OnLineSkipped;
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnLineSkipped()
        {
            ServiceLocator.GetService<IDialogueManager>()
                .GetComponentsInChildren<MonoBehaviour>()
                .OfType<ILineReader>()
                .FirstOrDefault().OnLineEndEvent -= OnLineSkipped;

            inFastForwardMode.SetValue(false);

            ServiceLocator.GetService<IInputManager>().Enable();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual string ApplyTextModifications(Line currentLine)
        {
            string preparedText = currentLine.Text;

            foreach (var modifier in textModifiers)
                preparedText = modifier.ApplyModification(preparedText, currentLine, box);

            return preparedText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="preparedText"></param>
        /// <returns></returns>
        protected virtual IEnumerator DisplayTextAnimation(string preparedText)
        {
            string textWithoutHTMLTags = Regex.Replace(preparedText, "<.*?>", string.Empty);

            for (int i = 0; i < textWithoutHTMLTags.Length && !skipText; i++)
            {
                if (i == 1 && !skipTextStarted)
                    StartCoroutine(SkipText());

                box.maxVisibleCharacters = i;

                if (!inFastForwardMode.Value)
                    yield return new WaitForSecondsRealtime(textDisplaySpeed * bufferTextMultiplier);
            }

            box.maxVisibleCharacters += 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator SkipText()
        {
            skipTextStarted = true;

            while (IsWaitingForClick())
                if (submit.action.triggered || inFastForwardMode.Value)
                    break;
                else
                    yield return null;

            skipTextStarted = false;

            skipText = true;
        }

        /// <inheritdoc/>
        public override bool ShouldProcess(Line currentLine)
        {
            foreach (var modifier in textModifiers)
                if (modifier is ILineRoutine routineCheck && routineCheck.ShouldProcess(currentLine))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual bool SkipDisplayingLine(Line currentLine)
        {
            foreach (var modifier in textModifiers)
                if (modifier.SkipLine(currentLine))
                    return true;

            return false;
        }
    }
}