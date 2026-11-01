//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public static class FlagTriggerExtensions
    {
        // If Particle Progression is working normally this code can be deleted.
        //public static bool ContainsFlagTriggerFor(this Dialogue dialogue, int chap, int objName)
        //{
        //    if (dialogue.FlagTrigger != null &&
        //        dialogue.FlagTrigger.Enabled && 
        //        dialogue.FlagTrigger.Objective == objName &&
        //        dialogue.FlagTrigger.Chapter == chap)
        //        return true;
        //    else if (dialogue != null && dialogue.DirectTo != null && dialogue.DirectTo.Enabled)
        //        return dialogue.DirectTo.NewDialogue.ContainsFlagTriggerFor(chap, objName);

        //    return false;
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public static bool HasUnTriggeredRequiredFlags<T>(this DialogueBase<T> dialogue)
        {
            if (dialogue != null && dialogue.FlagTrigger != null && dialogue.FlagTrigger.Enabled)
                foreach (var index in dialogue.FlagTrigger.Flags)
                    if (HasUnTriggeredRequiredFlag(dialogue, index))
                        return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool HasUnTriggeredRequiredFlag<T>(DialogueBase<T> dialogue, int index)
        {
            if (!dialogue.FlagTrigger.FlagIsOptional(index) && !dialogue.FlagTrigger.FlagHasBeenTriggered(index))
                return true;
            else if (dialogue.DirectTo != null && dialogue.DirectTo.Enabled)
                return dialogue.DirectTo.NewDialogue.HasUnTriggeredRequiredFlags();

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="flagIndex"></param>
        /// <returns></returns>
        public static bool FlagHasBeenTriggered(this TriggerFlag trigger, int flagIndex)
        => ServiceLocator.GetService<FlagTriggerHandlerBase>()
            .GetFlagFromIndexes(trigger.Chapter, trigger.Objective, flagIndex).triggered;

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="flagIndex"></param>
        /// <returns></returns>
        public static bool FlagIsOptional(this TriggerFlag trigger, int flagIndex)
            => ServiceLocator.GetService<FlagTriggerHandlerBase>()
            .GetFlagFromIndexes(trigger.Chapter, trigger.Objective, flagIndex).optional;
    }
}