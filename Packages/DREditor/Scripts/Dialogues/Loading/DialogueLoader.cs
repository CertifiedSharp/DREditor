//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueLoader : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public abstract DialogueBase<T>[] StartLoadThrough<T>(DialogueBase<T>[] dialogue) 
            where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogueCore"></param>
        /// <param name="resultingIndex"></param>
        public abstract IEnumerator LoadToDialogueLine<T>(DialogueCore currentDialogueCore, Action<int> resultingIndex)
            where T : Line;
    }
}