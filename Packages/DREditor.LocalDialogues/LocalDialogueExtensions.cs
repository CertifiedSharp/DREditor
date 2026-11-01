//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Progressing.Handling;
using DREditor.Services;

namespace DREditor.LocalDialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class LocalDialogueExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="conversations"></param>
        /// <returns></returns>
        public static bool AllDialogueTriggered(this List<LocalDialogue> conversations)
        {
            foreach (var dialogue in conversations)
                if (!dialogue.triggered)
                    return false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="conversations"></param>
        /// <returns></returns>
        public static bool ContainsTriggeredDialogue(this List<LocalDialogue> conversations)
        {
            foreach (var dialogue in conversations)
                if (dialogue.triggered)
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="conversations"></param>
        /// <returns></returns>
        public static bool HasUnTriggeredRequiredFlags(this List<LocalDialogue> conversations)
        {
            foreach (LocalDialogue localDialogue in conversations)
                if (localDialogue.HasUnTriggeredRequiredFlags())
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="localDialogue"></param>
        /// <returns></returns>
        public static bool HasUnTriggeredRequiredFlags(this LocalDialogue localDialogue)
        {
            if (localDialogue.dialogueIfFlagNotMet.HasUnTriggeredRequiredFlags())
                return true;

            if (!localDialogue.triggered && localDialogue.dialogue.HasUnTriggeredRequiredFlags())
                return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="localDialogues"></param>
        /// <returns></returns>
        public static bool CheckIfSaveDialogue(this List<LocalDialogue> localDialogues)
        {
            foreach (LocalDialogue localDialogue in localDialogues)
                if (ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<Line>(localDialogue.dialogue)
                    ||
                    ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<Line>(localDialogue.dialogueIfFlagNotMet))
                    return true;

            return false;
        }
    }
}