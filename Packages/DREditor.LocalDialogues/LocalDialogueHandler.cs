//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Progressing.Handling;
using DREditor.Services;
using System.Collections.Generic;

namespace DREditor.LocalDialogues
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<LocalDialogueHandlerBase>) + " for type: " + nameof(LocalDialogueHandlerBase) +
        "\n\n")]
    public class LocalDialogueHandler : LocalDialogueHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        protected LocalDialogue localDialogue = null;


        /// <inheritdoc/>
        public override void CheckBoolTrigger(List<BoolTrigger> boolTriggers)
        {
            foreach (BoolTrigger trigger in boolTriggers)
                if (BookOfTriggers.ContainsKey(trigger.actorName))
                    BookOfTriggers[trigger.actorName].SkipConversation(trigger.convoNum);
        }

        /// <inheritdoc/>
        public override Dialogue EvaluateDialogue(List<LocalDialogue> RoomConvo)
        {
            if (RoomConvo.Count == 0)
                return null;

            foreach (LocalDialogue localDialogue in RoomConvo)
                if (EvaluatedConversation(localDialogue) is Dialogue evaluated && evaluated != null)
                {
                    if (localDialogue.dialogueBoolTriggers.Count > 0)
                        CheckBoolTrigger(localDialogue.dialogueBoolTriggers);

                    return evaluated;
                }

            return RoomConvo[^1].dialogue; // Returns last Dialogue (After Chat)
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentDialogue"></param>
        /// <returns></returns>
        protected virtual Dialogue EvaluatedConversation(LocalDialogue currentDialogue)
        {
            localDialogue = currentDialogue;

            if (localDialogue != null && !localDialogue.triggered)
            {
                if (PassedRequiredFlag() is Dialogue flagResult && flagResult != null)
                    return flagResult;
                else
                    return TriggerResultingDialogue();
            }
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Dialogue PassedRequiredFlag()
        {
            if (localDialogue.requiredProgressionFlag != "")
                return EvaluateFlagResult();
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Dialogue EvaluateFlagResult()
        {
            if (ServiceLocator.GetService<FlagTriggerHandlerBase>().IsFlagTriggered(localDialogue.requiredProgressionFlag))
                return TriggerResultingDialogue();
            else
                return localDialogue.dialogueIfFlagNotMet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Dialogue TriggerResultingDialogue()
        {
            if (localDialogue.dialogue != null)
                localDialogue.SetTriggered();

            return localDialogue.dialogue;
        }
    }
}