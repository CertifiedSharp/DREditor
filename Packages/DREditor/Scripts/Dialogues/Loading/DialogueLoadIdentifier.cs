//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueLoadIdentifier : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected DialogueCore LoadDialogue = null;


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogues"></param>
        /// <returns></returns>
        public abstract bool CheckDialogues<T>(List<DialogueBase<T>> dialogues) where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public abstract bool CheckDialogue<T>(DialogueBase<T> dialogue) where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual DialogueCore GetLoadedDialogue() => LoadDialogue;


        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected abstract bool IsDialogueToLoad(DialogueCore dialogue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected abstract bool CheckIdentification(DialogueCore dialogue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        protected abstract void OnIdentificationMatched(DialogueCore dialogue);
    }
}