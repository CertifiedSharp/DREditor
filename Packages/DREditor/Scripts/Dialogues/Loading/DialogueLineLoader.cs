//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueLineLoader : LineLoader
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent loadingToLine = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentLineNum = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LoadThroughDialogueEvents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> OnLoadedThroughDialogues = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LoadToLineComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LoadToLineEvents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LoadToLineLastingComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LoadToLineEndComponents = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> OnLoadedLinePreparers = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> OnLoadToLineResolvers = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> LoadEndStateComponents = new();

        #endregion

        #region Load Through Lines

        /// <inheritdoc/>
        public override void LoadThroughLines(DialogueCore dialogue, int directNum)
        {
            for (int i = 0; i < dialogue.LineCount; i++)
            {
                currentLineNum.SetValue(i);

                LoadThroughLine(dialogue.GetLines<Line>()[i], directNum);
            }

            FinishedLoadingThrough(dialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        protected virtual void FinishedLoadingThrough(DialogueCore dialogue)
        {
            foreach (ILoadedThroughDialogue loadedThrough in OnLoadedThroughDialogues.Cast<ILoadedThroughDialogue>())
                loadedThrough.OnLoadedThroughDialogue(dialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="directNum"></param>
        protected virtual void LoadThroughLine(Line currentLine, int directNum)
        {
            TriggerHandlers(currentLine);

            TriggerDialogueEvents(currentLine, directNum);

            TriggerLastingDialogueEvents(currentLine);

            TriggerEndHandlers(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void TriggerHandlers(Line currentLine)
        {
            foreach (ILoadToLineable loadToLineable in LoadToLineComponents.Cast<ILoadToLineable>())
                loadToLineable.OnLoadToLine(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="directNum"></param>
        protected virtual void TriggerDialogueEvents(Line currentLine, int directNum)
        {
            foreach (ILoadThroughDialogueEvents loadThrough in LoadThroughDialogueEvents.Cast<ILoadThroughDialogueEvents>())
                loadThrough.LoadThroughDialogueEvents(currentLine, directNum);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void TriggerLastingDialogueEvents(Line currentLine)
        {
            foreach (ILoadToLineLastingEvent loadToLineableLasting in LoadToLineLastingComponents.Cast<ILoadToLineLastingEvent>())
                loadToLineableLasting.OnLoadToLineLasting(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void TriggerEndHandlers(Line currentLine)
        {
            foreach (ILoadToLineEnd loadLineEndable in LoadToLineEndComponents.Cast<ILoadToLineEnd>())
                loadLineEndable.OnLoadToLineEnd(currentLine);
        }

        #endregion

        #region Load to Line

        /// <inheritdoc/>
        public override int LoadToLine<T>(DialogueBase<T> dialogue, int lineIndex)
        {
            UnityEngine.Debug.Log($"REPORT: Load to Line {lineIndex} has started.");

            loadingToLine.SetValue(true);

            for (int i = 0; i < dialogue.Lines.Count; i++)
            {
                Line currentLine = dialogue.Lines[i];

                currentLineNum.SetValue(i);

                bool onLoadedLine = i == lineIndex;

                if (onLoadedLine)
                    return PrepareLoadedLine(currentLine, i);

                TriggerHandlers(currentLine);

                TriggerLoadToLineEvents(currentLine);

                TriggerLastingDialogueEvents(currentLine);

                TriggerEndHandlers(currentLine);
            }

            LoadToLineResolve();

            loadingToLine.SetValue(false);

            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void TriggerLoadToLineEvents(Line currentLine)
        {
            foreach (ILoadToLineEvents loadToLineEvent in LoadToLineEvents.Cast<ILoadToLineEvents>())
                loadToLineEvent.OnLoadToLineEvents(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        protected virtual int PrepareLoadedLine(Line currentLine, int lineIndex)
        {
            UnityEngine.Debug.Log("REPORT: Loaded to line dialogue was saved on: " + lineIndex);

            LoadToLinePrepare(currentLine);

            LoadToLineResolve();

            return lineIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void LoadToLinePrepare(Line currentLine)
        {
            foreach (IOnPrepareLoadedLine preparables in OnLoadedLinePreparers.Cast<IOnPrepareLoadedLine>())
                preparables.OnPrepareLoadedLine(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadToLineResolve()
        {
            foreach (ILoadToLineResolver resolver in OnLoadToLineResolvers.Cast<ILoadToLineResolver>())
                resolver.OnLoadedLineResolve();
        }

        #endregion

        #region Load End States

        /// <inheritdoc/>
        public override IEnumerator LoadEndStates(Line currentLine)
        {
            foreach (ILoadEndState loader in LoadEndStateComponents.Cast<ILoadEndState>())
                yield return loader.LoadEndStates(currentLine);
        }

        #endregion
    }
}