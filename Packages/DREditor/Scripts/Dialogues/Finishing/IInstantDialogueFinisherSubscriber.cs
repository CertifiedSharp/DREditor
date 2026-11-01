//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues.Finishing
{
    /// <summary>
    ///
    /// </summary>
    public interface IInstantDialogueFinisherSubscriber
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="function"></param>
        void SubscribeToOnFinishOfInstantDialogue(Action function);

        /// <summary>
        ///
        /// </summary>
        /// <param name="function"></param>
        void UnsubscribeToOnFinishOfInstantDialogue(Action function);
    }
}