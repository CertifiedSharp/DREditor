//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using System.Collections.Generic;

namespace DREditor.LocalDialogues
{
    /// <summary>
    ///
    /// </summary>
    public abstract class LocalDialogueHandlerBase : ServiceHolder<LocalDialogueHandlerBase>
    {
        /// <summary>
        ///
        /// </summary>
        public readonly static Dictionary<string, ISkipConversation> BookOfTriggers = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="boolTriggers"></param>
        public abstract void CheckBoolTrigger(List<BoolTrigger> boolTriggers);

        /// <summary>
        ///
        /// </summary>
        /// <param name="roomConversations"></param>
        /// <returns></returns>
        public abstract Dialogue EvaluateDialogue(List<LocalDialogue> roomConversations);
    }
}