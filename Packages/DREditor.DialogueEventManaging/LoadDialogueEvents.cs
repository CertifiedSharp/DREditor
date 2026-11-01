//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    public class LoadDialogueEvents : FilteredDialogueEventManager, ILoadThroughDialogueEvents, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string directToLoadKey = "DirectToNum";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string transitionLoadKey = "TransitionNum";


        /// <summary>
        ///
        /// </summary>
        protected int saveDirectToNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected int saveTransitionNum = 0;


        /// <inheritdoc/>
        public virtual void LoadThroughDialogueEvents(Line currentLine, int directNum) => TriggerDialogueEvents(currentLine, directNum);


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="directNum"></param>
        protected virtual void TriggerDialogueEvents(Line currentLine, int directNum)
        {
            if (saveTransitionNum > 0)
                TriggerLineEventsFromEndEvents(currentLine, directNum);
            else
                TriggerDialogueLineEvents(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="directNum"></param>
        protected virtual void TriggerLineEventsFromEndEvents(Line currentLine, int directNum)
        {
            if (saveDirectToNum > 0 && directNum >= saveTransitionNum)
                TriggerDialogueLineEvents(currentLine);
        }
        

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas)
        {
            saveTransitionNum = saveDatas.Get<int>(transitionLoadKey);

            saveDirectToNum = saveDatas.Get<int>(directToLoadKey);
        }
    }
}