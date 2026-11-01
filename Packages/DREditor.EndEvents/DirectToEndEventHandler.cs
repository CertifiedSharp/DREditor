//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) + ", " + nameof(IClearDialogueSaveData) + ", " + 
        nameof(IDRESaveable) + ", " + nameof(IDRELoadable) + ", " + nameof(ISceneEventListener) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class DirectToEndEventHandler : DialogueComponent, IDialogueEndEvent, IClearDialogueSaveData,
        IDRESaveable, IDRELoadable, ISceneEventListener, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent updateDirectTo = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent fastForwardModeWasEnabled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string directToNumSaveKey = "DirectToNum";


        /// <summary>
        ///
        /// </summary>
        protected int saveDirectToNum = 0;


        protected virtual void OnEnable() => updateDirectTo.RegisterListener(this);

        protected virtual void OnDisable() => updateDirectTo.UnregisterListener(this);


        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
            => EvaluateDirectTo(currentDialogue, interruptDialogue);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="interruptDialogue"></param>
        /// <returns></returns>
        protected virtual bool EvaluateDirectTo<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue) 
            where T : Line
        {
            bool isValidDirectTo = currentDialogue.DirectTo != null && currentDialogue.DirectTo.Enabled;

            if (isValidDirectTo)
                OnDirectToValid(currentDialogue, interruptDialogue);
            else
                ResolveFFVariable();

            return isValidDirectTo;
        }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="interruptDialogue"></param>
        protected virtual void OnDirectToValid<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
        {
            DialogueBase<T>[] nextDialogueToBePlayed = new DialogueBase<T>[1];

            nextDialogueToBePlayed[0] = currentDialogue.DirectTo.NewDialogue;

            OnEventRaised();

            fastForwardModeWasEnabled.SetValue(true);

            interruptDialogue(nextDialogueToBePlayed);
        }


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            if (!ServiceLocator.GetService<IDialogueManager>().GetComponentsInChildren<DialogueComponent>().Contains(this))
                return;

            saveData.Set(directToNumSaveKey, saveDirectToNum);
        }

        /// <inheritdoc/>
        public virtual void ClearSaveData() => saveDirectToNum = 0;

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => saveDirectToNum = saveDatas.Get<int>(directToNumSaveKey);

        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            if (!ServiceLocator.GetService<IDialogueManager>().GetComponentsInChildren<DialogueComponent>().Contains(this))
                return;

            saveDirectToNum++;
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => ResolveFFVariable();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveFFVariable() => fastForwardModeWasEnabled.SetValue(false);
    }
}