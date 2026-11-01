//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class LineReader : DialogueComponent, IClearDialogueSaveData
    {
        /// <inheritdoc/>
        public abstract void ClearSaveData();


        /// <summary>
        ///
        /// </summary>
        public abstract void ClearSaveDataOnDialogueStart();

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDialogueCore"></param>
        /// <param name="currentLine"></param>
        /// <param name="exitParent"></param>
        /// <param name="continueLine"></param>
        /// <returns></returns>
        public abstract IEnumerator PlayDialogueLine<T>(DialogueBase<T> currentDialogueCore, Line currentLine, 
            Action exitParent = null, Action continueLine = null) where T : Line;
    }
}