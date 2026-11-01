//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System;
using System.Collections;

namespace DREditor.Choices
{
    /// <summary>
    ///
    /// </summary>
    public interface IChoiceManager
    {
        /// <summary>
        ///
        /// </summary>
        IEnumerator ExecuteChoiceHandling(Dialogue currentDialogue, Action exitParentRoutine = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="actionArray"></param>
        void SubscribeAllChoices(Action[] actionArray);

        /// <summary>
        ///
        /// </summary>
        void SubscribeChoice(int index, Action action);

        /// <summary>
        ///
        /// </summary>
        void UnsubscribeChoice(int index, Action action);

        /// <summary>
        ///
        /// </summary>
        void UnsubscribeAllChoices();

        /// <summary>
        ///
        /// </summary>
        void ClearChoiceText();
    }
}