//Author: Happy30
//Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.EndEvents.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// End event for the sole purpose of directing the game to another dialogue asset in order to continue the conversation.
    /// </summary>
    [System.Serializable]
    public class DirectTo : EndEventBase
    {
        /// <summary>
        /// The dialogue the game should direct to in order to continue the conversation.
        /// </summary>
        public DialogueCore NewDialogue;

        /// <summary>
        /// The line index to start on when moving to the new dialogue when this event is processed.
        /// </summary>
        public int NewDialogueIndex;


        public DirectTo() { }

        public DirectTo(DialogueCore NewDialogue) => this.NewDialogue = NewDialogue;


        /// <summary>
        /// Sets the dialogue the game should direct to in order to continue the conversation.
        /// </summary>
        /// <param name="NewDialogue">The dialogue the game should direct to in order to continue the conversation.</param>
        public virtual void SetNewDialogue(DialogueCore NewDialogue) => this.NewDialogue = NewDialogue;

        /// <summary>
        /// Sets the line index to start on when moving to the new dialogue when this event is processed.
        /// </summary>
        /// <param name="NewDialogueIndex">The line index to start on when moving to the new dialogue when this event is processed.</param>
        public virtual void SetNewDialogueIndex(int NewDialogueIndex) => this.NewDialogueIndex = NewDialogueIndex;
    }
}
