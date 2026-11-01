//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
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
    /// <remarks></remarks>
    public class DialogueAssetReader : DialogueReader, IToTitleScreen
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] DialogueCore currentlyPlayingDialogue;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected DialogueEndEventHandler DialogueEndEventHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueEnder DialogueEnder = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueDataClearer DialogueClearer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueLoader DialogueLoader = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentLineNum = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent updateDirectTo = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLeaving = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent saveDialogueName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LineReader LineReader = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> DialogueReaderInitializeComponents = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DialogueComponent> DialogueReaderCleanUpComponents = new();


        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IDialogueInteruptable> interuptables;

        /// <summary>
        ///
        /// </summary>
        protected int lastLineNum = -1;

        #endregion

        #region Awake/Enable/Disable/Reset


        protected virtual void Awake() => interuptables = transform.root.GetComponentsInChildren<IDialogueInteruptable>();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => SubscribeToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToEvents()
        {
            foreach (var interuptable in interuptables)
                interuptable.OnInteruptWithNewDialogue += PlayDialogues;

            DialogueEndEventHandler.SubscribeToCleanUp(CleanUp);
        }

        
        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnsubscribeToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToEvents()
        {
            foreach (var interuptable in interuptables)
                interuptable.OnInteruptWithNewDialogue -= PlayDialogues;

            DialogueEndEventHandler.UnsubscribeToCleanUp(CleanUp);
        }

        #endregion

        #region Play Dialogues

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogues"></param>
        /// <remarks></remarks>
        public virtual void PlayDialogues(DialogueCore[] dialogues)
        {
            List<Dialogue> dias = new();

            foreach(DialogueCore dialogue in dialogues)
                if (dialogue is Dialogue dia)
                    dias.Add(dia);
                else
                    UnityEngine.Debug.LogWarning("ALERT: ISSUE PLAYING DIALOGUE WAS NOT OF DIALOGUE TYPE");

            StartCoroutine(PlayDialoguesRoutine(dias.ToArray()));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogues"></param>
        /// <param name="inLeave"></param>
        public override void PlayDialogues<T>(DialogueBase<T>[] dialogues, bool inLeave = false) => 
            StartCoroutine(PlayDialoguesRoutine<T>(dialogues, inLeave));

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogues"></param>
        /// <param name="intendingToLeave"></param>
        /// <returns></returns>
        protected virtual IEnumerator PlayDialoguesRoutine<T>(DialogueBase<T>[] dialogues, bool intendingToLeave = false) 
            where T : Line
        {
            isLeaving.SetValue(intendingToLeave);

            InitializeDialogueStarting();

            for (int i = 0; i < dialogues.Length; i++)
            {
                DialogueBase<T> currentDialogue = dialogues[i];

                if (i != 0)
                    updateDirectTo.Raise();

                bool stopParent = false;

                yield return PlayDialogue(currentDialogue, () => stopParent = true);

                LineReader.ClearSaveData();

                if (stopParent)
                    yield break;
            }

            CleanUp();

            DialogueEnder.StopDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDialogueStarting()
        {
            if (!isLoadingFile.Value)
                LineReader.ClearSaveDataOnDialogueStart();

            foreach (IDialogueReaderInitialize initializer in DialogueReaderInitializeComponents.Cast<IDialogueReaderInitialize>())
                initializer.OnDialogueReaderInitialize();
        }

        /// <summary>
        ///
        /// </summary>
        public override void CleanUp()
        {
            DialogueClearer.ClearSaveData();

            foreach (IDialogueReaderCleanUp cleanUpComponent in DialogueReaderCleanUpComponents.Cast<IDialogueReaderCleanUp>())
                cleanUpComponent?.OnDialogueReaderCleanUp();
        }

        #endregion

        #region Play Dialogue

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="exitParentRoutine"></param>
        protected virtual IEnumerator PlayDialogue<T>(DialogueBase<T> currentDialogue, Action exitParentRoutine = null) 
            where T : Line
        {
            currentlyPlayingDialogue = currentDialogue;

            for (int i = 0; i < currentDialogue.Lines.Count; i++)
            {
                Line currentLine = currentDialogue.Lines[i];

                if (isLoadingFile.Value)
                    yield return DialogueLoader.LoadToDialogueLine<T>(currentDialogue,
                        loadedIndex => { i = loadedIndex; currentLine = currentDialogue.Lines[i]; });

                SetLineNumber(i);


                bool exitEarly = false;

                bool continueLine = false;

                yield return LineReader.PlayDialogueLine(currentDialogue, currentLine, () => exitEarly = true, () => continueLine = true);

                if (exitEarly)
                {
                    exitParentRoutine?.Invoke();

                    yield break;
                }

                if (continueLine)
                    continue;
            }

            // The dialogue reader has finished reading the dialogue lines.

            SetLineOutOfBounds();


            bool exitParent = false;

            yield return DialogueEndEventHandler.EndDialogueEventOptions(currentDialogue, () => exitParent = true);

            if (exitParent)
                exitParentRoutine?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLineNum"></param>
        protected virtual void SetLineNumber(int currentLineNum) => this.currentLineNum.SetValue(currentLineNum);

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetLineOutOfBounds()
        {
            lastLineNum = currentLineNum.Value;

            SetLineNumber(-1); 
        }

        #endregion

        #region Additional Public Functions

        /// <inheritdoc/>
        public override void MidDialogueEnterance<T>(DialogueBase<T>[] dialogues)
        {
            if (dialogues.Length == 0)
            {
                UnityEngine.Debug.LogWarning("ALERT: Tried to send MidDialogue without dialogues!");

                return;
            }

            DialogueClearer.ClearSaveData();

            saveDialogueName.SetValue(dialogues[0].name);

            PlayDialogues<T>(dialogues);
        }

        /// <inheritdoc/>
        public override bool IsOnLastLine() => 
            GetCurrentDialogue() != null && GetCurrentLineNum() == GetCurrentDialogue().LineCount - 1;

        /// <inheritdoc/>
        public override DialogueCore GetCurrentDialogue() => currentlyPlayingDialogue;

        /// <inheritdoc/>
        public override int GetCurrentLineNum() => currentLineNum.Value;

        /// <inheritdoc/>
        public override Line GetCurrentLine()
        {
            int currentLineNumber = GetCurrentLineNum();

            bool linesAreBeingRead = currentLineNumber != -1;

            int lineNumber = linesAreBeingRead ? currentLineNumber : lastLineNum;

            if (linesAreBeingRead)
                lastLineNum = -1;

            return GetCurrentDialogue().GetLine(lineNumber);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void ToTitleScreen()
        {
            SetLineOutOfBounds();

            StopAllCoroutines();
        }

        #endregion
    }
}