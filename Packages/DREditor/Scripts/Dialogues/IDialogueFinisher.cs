//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public interface IDialogueFinisher
    {
        /// <summary>
        ///
        /// </summary>
        event Action OnDialogueEnd;


        /// <summary>
        ///
        /// </summary>
        void CallOnDialogueEnd();

        /// <summary>
        ///
        /// </summary>
        void CallOnFinishedDialogue();

        /// <summary>
        ///
        /// </summary>
        void FinishDialogue();

        /// <summary>
        ///
        /// </summary>
        /// <param name="finishDialogue"></param>
        IEnumerator FinishDialogueRoutine(bool finishDialogue = true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="function"></param>
        void SubscribeToOnFinishOfDialogue(Action function);

        /// <summary>
        ///
        /// </summary>
        /// <param name="function"></param>
        void UnsubscribeToOnFinishOfDialogue(Action function);
    }
}