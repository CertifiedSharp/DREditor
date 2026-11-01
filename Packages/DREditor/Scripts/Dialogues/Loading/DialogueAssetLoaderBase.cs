//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public class DialogueAssetLoaderBase : DialogueLoader, IToTitleScreen
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueLoadIdentifier DialogueLoadIdentifier = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected LineLoader LineLoader = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveDialogueNameKey = "DialogueName";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveDirectToNumKey = "DirectToNum";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveLineNumKey = "LineNum";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveConditionalKey = "Conditional";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveChoicesKey = "Choices";


        /// <summary>
        ///
        /// </summary>
        protected int directNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected int conditionNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected int choiceCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected string DialogueName;

        /// <summary>
        ///
        /// </summary>
        protected int DirectToNum;

        /// <summary>
        ///
        /// </summary>
        protected int LineNum;

        /// <summary>
        ///
        /// </summary>
        protected List<bool> Conditionals;

        /// <summary>
        ///
        /// </summary>
        protected List<int> Choices;

        #endregion

        #region Loading Dialogue

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueData"></param>
        protected virtual void SetDialogueData(object dialogueData)
        {
            try
            {
                Dictionary<string, object> saveDatas = (Dictionary<string, object>)dialogueData;

                DialogueName = saveDatas.Get<string>(saveDialogueNameKey);

                DirectToNum = saveDatas.Get<int>(saveDirectToNumKey);

                LineNum = saveDatas.Get<int>(saveLineNumKey);

                Conditionals = saveDatas.Get<List<bool>>(saveConditionalKey);

                Choices = saveDatas.Get<List<int>>(saveChoicesKey);
            }
            catch
            {
                UnityEngine.Debug.LogError("The sent save data to load the dialogue process did not cast to the correct type!");
            }
        }

        #endregion

        #region Load Through Process

        /// <inheritdoc/>
        public override DialogueBase<T>[] StartLoadThrough<T>(DialogueBase<T>[] dialogues)
        {
            foreach(var dialogue in dialogues)
                if (LoadThrough(dialogue) is DialogueBase<T>[] result)
                {
                    if (result.Length > 0 && Array.IndexOf(dialogues, result[0]) is int matchIndex && matchIndex >= 0)
                        return dialogues.Skip(matchIndex).ToArray();
                    else
                        return result;
                }
                else if (dialogues.Length > 1)
                    directNum++;
                else
                    DisplayLoadFailureLogs(dialogue);

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual DialogueBase<T>[] LoadThrough<T>(DialogueBase<T> dialogue)
            where T : Line
        {
            UnityEngine.Debug.Log($"REPORT: {gameObject.name}: Starting to LoadThrough Dialogue starting with: " + dialogue.name);

            if (OnCorrectDialogueAsset())
                return ReturnDialogues(dialogue);

            LineLoader.LoadThroughLines(dialogue, directNum);

            return ReturnNextDialogueFromEndEvents(dialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnCorrectDialogueAsset()
        {
            bool directToNumCorrect = directNum == DirectToNum;

            bool conditionNumCorrect = Conditionals != null && conditionNum == Conditionals.Count;

            bool choiceCountNumCorrect = Choices != null && choiceCount == Choices.Count;

            bool onCorrectDialogueAsset = directToNumCorrect && conditionNumCorrect && choiceCountNumCorrect;

            return onCorrectDialogueAsset;
        }

        #endregion

        #region Load Dialogue Through End Events

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogueCore"></param>
        /// <returns></returns>
        protected virtual DialogueBase<T>[] ReturnNextDialogueFromEndEvents<T>(DialogueBase<T> dialogueCore)
            where T : Line
        {
            bool directToNumIncorrect = directNum != DirectToNum;

            int conditionalCount = Conditionals != null ? Conditionals.Count : 0;

            bool conditionalCountIncorrect = conditionalCount > 0 && conditionNum != conditionalCount;

            bool hasMadeChoices = Choices != null && Choices.Count > 0;

            if (directToNumIncorrect)
                return LoadNextDirectTo(dialogueCore);
            else if (conditionalCountIncorrect)
                return LoadNextSaveCondition(dialogueCore);
            else if (hasMadeChoices)
                return LoadNextSaveChoice(dialogueCore);
            else
                return ReturnDialogues(dialogueCore.DirectTo.NewDialogue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual DialogueBase<T>[] LoadNextDirectTo<T>(DialogueBase<T> dialogue)
            where T : Line
        {
            UnityEngine.Debug.Log($"REPORT: Reached end of Load Through: Has DirectTo: {directNum} out of {DirectToNum}");

            bool directToDialogueIsValid = dialogue.DirectTo.NewDialogue != null;

            bool conditionNumNotReached = conditionNum != Conditionals.Count;

            bool choiceNumNotReached = Choices != null && Choices.Count > 0;

            if (directToDialogueIsValid)
                directNum++;

            if (directToDialogueIsValid)
                return LoadThrough(dialogue.DirectTo.NewDialogue);
            else if (conditionNumNotReached) // Pretty sure this can't be reached but putting it here to cover
                return LoadNextSaveCondition(dialogue);
            else if (choiceNumNotReached)
                return LoadNextSaveChoice(dialogue);
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual DialogueBase<T>[] LoadNextSaveCondition<T>(DialogueBase<T> dialogue)
            where T : Line
        {
            UnityEngine.Debug.Log("Reached end of Load Through: Has Save Condition");

            conditionNum++;

            if (Conditionals[conditionNum - 1])
                return LoadThrough(dialogue.Variable.NextDialogueTrue);
            else
                return LoadThrough(dialogue.Variable.NextDialogueFalse);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual DialogueBase<T>[] LoadNextSaveChoice<T>(DialogueBase<T> dialogue)
            where T : Line
        {
            UnityEngine.Debug.Log("Reached end of Load Through: Loading next save choice: " + choiceCount);

            if (dialogue is Dialogue dia && dia.Choices[Choices[choiceCount]].NextDialogue is DialogueBase<T> viableDialogue)
            {
                choiceCount++;

                return LoadThrough(viableDialogue);
            }
            else
                UnityEngine.Debug.LogError("The dialogue to load the next saved choice or dialogue asset containing choice data was not " +
                    "of a valid Type.");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        protected virtual void DisplayLoadFailureLogs<T>(DialogueBase<T> dialogue)
            where T : Line
        {
            UnityEngine.Debug.LogWarning("ALERT: Reached end of Load Through: SOMETHING WENT WRONG Displaying Data.");

            UnityEngine.Debug.Log("Current Dia: " + dialogue.name);

            UnityEngine.Debug.Log("Direct Nums: " + directNum + " Save DirectTo Num: " + DirectToNum);

            UnityEngine.Debug.Log("Direct Condition Num: " + conditionNum + " Condition Count: " + Conditionals.Count);

            UnityEngine.Debug.Log("DirectTo New Dialogue is something?: " + (dialogue.DirectTo.NewDialogue != null));

            UnityEngine.Debug.Log("SaveDirectTo Num: " + DirectToNum);

            UnityEngine.Debug.Log("Conditional Count Num: " + Conditionals.Count);

            UnityEngine.Debug.Log("SaveLineNum: " + LineNum);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual DialogueBase<T>[] ReturnDialogues<T>(DialogueBase<T> dialogue)
            where T : Line
        {
            UnityEngine.Debug.Log("Completed Loading Through previous dialogues, Playing Dialogue now.");

            DialogueBase<T>[] dialogues = new DialogueBase<T>[1];

            dialogues[0] = dialogue;

            return dialogues;
        }

        #endregion

        #region Load To Dialogue Line

        /// <inheritdoc/>
        public override IEnumerator LoadToDialogueLine<T>(DialogueCore currentDialogueCore, Action<int> resultingIndex)
        {
            DialogueBase<T> currentDialogue = currentDialogueCore as DialogueBase<T>;

            int i = LineNum != 0 ? LineLoader.LoadToLine(currentDialogue, LineNum) : 0;

            UnityEngine.Debug.Log($"REPORT: {gameObject.name} Loaded to Line {LineNum} Index: " + i);

            yield return LineLoader.LoadEndStates(currentDialogue.Lines[i]);

            resultingIndex?.Invoke(i);
        }

        #endregion

        #region Additional Public Functions

        /// <inheritdoc/>
        public virtual void ToTitleScreen()
        {
            directNum = 0;

            conditionNum = 0;

            choiceCount = 0;
        }

        #endregion
    }
}