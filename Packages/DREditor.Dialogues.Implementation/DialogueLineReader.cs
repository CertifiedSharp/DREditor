//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(LineReader) + ", " + nameof(ILineReader) + ", " + nameof(IDRESaveable) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    public class DialogueLineReader : LineReader, ILineReader, IDRESaveable, IDRELoadable
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent dialogueLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineStartComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineLastingEventComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineProcessComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineTextDisplayComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LinePostTextDisplayComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineWaitForClickComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LineEndComponents = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string currentLineNumberSaveKey = "LineNum";


        public event Action OnLineStart;

        public event Action OnLineEndEvent;


        /// <summary>
        ///
        /// </summary>
        protected int saveLineNum = 0;

        #endregion

        #region Dialogue Line Reading
        
        /// <inheritdoc/>
        public override IEnumerator PlayDialogueLine<T2>(DialogueBase<T2> currentDialogueCore, Line currentLine, 
            Action exitParentRoutine = null, Action continueLine = null)
        {
            OnLineStart?.Invoke();

            InvokeLineStartProcesses(currentLine);

            bool skipLine = false;

            yield return InvokeLineLastingEvents(currentLine, () => { skipLine = true; });

            if (skipLine)
            {
                UpdateAndContinueLine(continueLine);

                yield break;
            }


            yield return InvokeLineProcesses(currentLine);

            if (DialogueLineIsEmpty(currentLine))
            {
                UpdateAndContinueLine(continueLine);

                yield break;
            }

            yield return InvokeLineTextDisplayComponents(currentLine);

            if (isLoadingFile.Value)
                yield return WaitForGameFileToFinishLoading();


            InvokeLinePostTextDisplayComponents(currentLine);


            bool exitParent = false;

            yield return InvokeWaitForClickComponents(currentDialogueCore, currentLine, () => { exitParent = true; });

            if (exitParent)
            {
                exitParentRoutine?.Invoke();

                yield break;
            }


            hasMenuAccess.SetValue(false);

            yield return InvokeLineEndComponents(currentLine);

            SetSaveLineNumber(saveLineNum + 1);

            OnLineEndEvent?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="continueLine"></param>
        protected virtual void UpdateAndContinueLine(Action continueLine)
        {
            SetSaveLineNumber(saveLineNum + 1);

            continueLine?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="saveLineNum"></param>
        protected virtual void SetSaveLineNumber(int saveLineNum) => this.saveLineNum = saveLineNum;

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void InvokeLineStartProcesses(Line currentLine)
        {
            foreach (var starter in LineStartComponents)
                if (starter is ILineStarter lineStarter)
                    lineStarter.LineStartProcess(currentLine);
                else
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {starter.GetType().Name} on {starter.gameObject.name} " +
                        $"was not a valid {nameof(ILineStarter)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LineStartComponents)} list on the line reader.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="continueLine"></param>
        protected virtual IEnumerator InvokeLineLastingEvents(Line currentLine, Action continueLine)
        {
            foreach (var lastingEvent in LineLastingEventComponents)
            {
                if (lastingEvent is ILineLastingEvent lineLastingEvent)
                {
                    bool skipLine = false;

                    yield return lineLastingEvent.LineLastingEvaluation(currentLine, () => { skipLine = true; });

                    if (skipLine)
                    {
                        continueLine?.Invoke();

                        yield break;
                    }
                }
                else
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {lastingEvent.GetType().Name} on " +
                        $"{lastingEvent.gameObject.name} was not a valid {nameof(ILineLastingEvent)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LineLastingEventComponents)} list on the line reader.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual bool DialogueLineIsEmpty(Line currentLine) => currentLine.Text == "" && !isLoadingFile.Value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator InvokeLineProcesses(Line currentLine)
        {
            foreach (var processor in LineProcessComponents)
            {
                if (processor is ILineProcessor lineProcessor)
                    lineProcessor.LineProcess(currentLine);

                if (processor is ILineProcessorRoutine lineRoutine && lineRoutine.ShouldProcess(currentLine))
                    yield return lineRoutine.LineProcessRoutine(currentLine);

                if (processor is not ILineProcessor && processor is not ILineProcessorRoutine)
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {processor.GetType().Name} on {processor.gameObject.name} " +
                        $"was not a valid {nameof(ILineProcessor)} or {nameof(ILineProcessorRoutine)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LineProcessComponents)} list on the line reader.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <remarks></remarks>
        protected virtual IEnumerator InvokeLineTextDisplayComponents(Line currentLine)
        {
            foreach (var processor in LineTextDisplayComponents)
                if (processor is ILineTextDisplay lineProcessor)
                    lineProcessor.LineTextDisplay(currentLine);
                else if (processor is ILineTextDisplayRoutine lineRoutine && lineRoutine.ShouldProcess(currentLine))
                    yield return lineRoutine.LineTextDisplay(currentLine);
                else if (processor is not ILineTextDisplay && processor is not ILineTextDisplayRoutine)
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {processor.GetType().Name} on {processor.gameObject.name} " +
                        $"was not a valid {nameof(ILineTextDisplay)} or {nameof(ILineTextDisplayRoutine)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LineTextDisplayComponents)} list on the line reader.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForGameFileToFinishLoading()
        {
            UnityEngine.Debug.LogWarning($"REPORT: {ToString()}: Noticed Dialogue has Finished Loading.");

            dialogueLoaded.SetValue(true);

            yield return new WaitUntil(() => !isLoadingFile.Value);

            AudioListener.volume = 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void InvokeLinePostTextDisplayComponents(Line currentLine)
        {
            foreach (var postTextDisplay in LinePostTextDisplayComponents)
                if (postTextDisplay is ILinePostTextDisplay linePostTextDisplay)
                    linePostTextDisplay.LinePostTextProcess(currentLine);
                else
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {postTextDisplay.GetType().Name} on " +
                        $"{postTextDisplay.gameObject.name} was not a valid {nameof(ILinePostTextDisplay)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LinePostTextDisplayComponents)} list on the line reader.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDialogue"></param>
        /// <param name="currentLine"></param>
        /// <param name="exitParentRoutine"></param>
        protected virtual IEnumerator InvokeWaitForClickComponents<T>(DialogueBase<T> currentDialogue,
            Line currentLine, Action exitParentRoutine)
            where T : Line
        {
            foreach (var waitForClick in LineWaitForClickComponents)
            {
                if (waitForClick is ILineWaitForClick lineWaitForClick)
                {
                    if (inFastForwardMode.Value && lineWaitForClick.SkipOnFastForward)
                        continue;

                    bool exitParent = false;

                    yield return lineWaitForClick.LineWaitForClick(currentDialogue, currentLine, () => { exitParent = true; });

                    if (exitParent)
                    {
                        exitParentRoutine?.Invoke();

                        yield break;
                    }
                }
                else
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {waitForClick.GetType().Name} on " +
                        $"{waitForClick.gameObject.name} was not a valid {nameof(ILineWaitForClick)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LineWaitForClickComponents)} list on the line reader.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <remarks></remarks>
        protected virtual IEnumerator InvokeLineEndComponents(Line currentLine)
        {
            foreach (var endProcessor in LineEndComponents)
                if (endProcessor is ILineEndProcess lineProcessor)
                    lineProcessor.LineEndProcess(currentLine);
                else if (endProcessor is ILineEndProcessRoutine lineRoutine)
                    yield return lineRoutine.LineEndProcess(currentLine);
                else if (endProcessor is not ILineTextDisplay && endProcessor is not ILineTextDisplayRoutine)
                    UnityEngine.Debug.LogWarning($"ALERT: The component: {endProcessor.GetType().Name} on " +
                        $"{endProcessor.gameObject.name} was not a valid " +
                        $"{nameof(ILineEndProcess)} or {nameof(ILineEndProcessRoutine)}!" +
                        $"\nPROPOSAL: Remove this object from the {nameof(LineEndComponents)} list on the line reader.");
        }

        #endregion

        #region Additional public Functions

        /// <inheritdoc/>
        public override void ClearSaveDataOnDialogueStart() => SetSaveLineNumber(0);

        /// <inheritdoc/>
        public override void ClearSaveData() => ClearSaveDataOnDialogueStart();

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas)
        {
            if (!ServiceLocator.GetService<IDialogueManager>().GetComponentsInChildren<DialogueComponent>().Contains(this))
                return;

            saveDatas.Set(currentLineNumberSaveKey, saveLineNum);
        }

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) 
            => SetSaveLineNumber(saveDatas.Get<int>(currentLineNumberSaveKey));

        #endregion
    }
}