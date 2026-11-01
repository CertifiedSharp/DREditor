//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ITextDisplayerHandler) + ", " + nameof(ILineTextDisplayRoutine) +
        "\n\n")]
    public class DialogueLineTextDisplayer : DialogueComponent, ITextDisplayerHandler, ILineTextDisplayRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextDisplayerBase TextDisplayer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI dialogueBox = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObjectWithEvent globalBox = null;

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
        [SerializeField] protected BoolWithEvent inCGMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inVideoMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgDialogueUIVisible = null;


        /// <summary>
        ///
        /// </summary>
        protected TMP_Text initializedGlobalBox;


        protected virtual void OnEnable()
        {
            if (globalBox != null)
                globalBox.AddListener(OnGlobalBoxUpdated);

            if (globalBox.Value != null)
                OnGlobalBoxUpdated(globalBox.Value);
        }

        protected virtual void OnDisable()
        {
            if (globalBox != null)
                globalBox.RemoveListener(OnGlobalBoxUpdated);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="globalBox"></param>
        protected virtual void OnGlobalBoxUpdated(GameObject globalBox)
        {
            if (globalBox != null)
                globalBox.TryGetComponent(out initializedGlobalBox);
        }


        /// <inheritdoc/>
        public virtual TMP_Text GetTextBox() => dialogueBox != null ? dialogueBox : initializedGlobalBox;

        /// <inheritdoc/>
        public virtual void ClearText() => TextDisplayer.ClearText();

        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine)
        {
            if (TextDisplayer.ShouldProcess(currentLine))
                return true;

            bool result = !SkipDisplayingText() && !inFastForwardMode.Value;

            if (inFastForwardMode.Value && !SkipDisplayingText())
            {
                ForceRegisterActiveService();

                TextDisplayer.DisplayText(GetTextBox(), currentLine);
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual IEnumerator LineTextDisplay(Line currentLine)
        {
            if (SkipDisplayingText())
                yield break;

            ForceRegisterActiveService();

            DisplayTextProcess(currentLine);

            yield return WaitForTextToFinishDisplaying();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkipDisplayingText() => inCGMode.Value || inVideoMode.Value;


        /// <summary>
        ///
        /// </summary>
        protected virtual void ForceRegisterActiveService()
        {
            if (!ServiceLocator.HasService<ITextDisplayerHandler>())
                ServiceLocator.Register<ITextDisplayerHandler>(this);
            else if (ServiceLocator.GetService<ITextDisplayerHandler>() != (ITextDisplayerHandler)this)
                ReplaceActiveService();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ReplaceActiveService()
        {
            ServiceLocator.Unregister(ServiceLocator.GetService<ITextDisplayerHandler>());

            ServiceLocator.Register<ITextDisplayerHandler>(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        protected virtual void DisplayTextProcess(Line line)
        {
            if (!cgDialogueUIVisible.Value && (inCGMode.Value || inVideoMode.Value))
                cgDialogueUIVisible.SetValue(true);

            TextDisplayer.DisplayText(GetTextBox(), line);

            TextDisplayer.SetWaitingForClick(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForTextToFinishDisplaying()
        {
            while (TextDisplayer.IsWaitingForClick() && !inFastForwardMode.Value)
                yield return null;
        }
    }
}