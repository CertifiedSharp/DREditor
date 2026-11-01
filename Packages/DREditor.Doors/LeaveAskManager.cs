//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
    public abstract class LeaveAskManager : ServiceHolder<LeaveAskManager>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract DialogueCore GetLeaveAskDialogue();
    }
}