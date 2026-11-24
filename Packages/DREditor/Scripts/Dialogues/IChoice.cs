//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.Choices.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// Interface for choice data during gameplay where the player must make a choice.
    /// </summary>
    public interface IChoice
    {
        /// <summary>
        /// The text displayed to the UI for this choice.
        /// </summary>
        string ChoiceText { get; set; }
    }
}