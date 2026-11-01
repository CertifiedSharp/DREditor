//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class LineLoader : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="directNum"></param>
        public abstract void LoadThroughLines(DialogueCore dialogue, int directNum);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        public abstract int LoadToLine<T>(DialogueBase<T> dialogue, int lineIndex) where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        public abstract IEnumerator LoadEndStates(Line currentLine);
    }
}