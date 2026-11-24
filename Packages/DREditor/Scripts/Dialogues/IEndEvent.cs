//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues
{
    /* ALERT: The following should be applied to this script after DREditor Ver.0.1.12 :
     * Insead of EndEventBase investigate just using IEndEvent.
     * Investigate moving this script to DREditor.EndEvents.
     * Remember to apply the MovedFrom Attribute properly if this script is moved.
     * If this is moved, also make sure that the Dialogue Event converter tool converts this stuff properly.
    */
    /// <summary>
    /// Interface for data events that are intended to be processed when all dialogue asset lines have completed being played.
    /// </summary>
    public interface IEndEvent
    {
        /// <summary>
        /// Whether or not this end event is valid to be processed.
        /// </summary>
        /// <returns>True if this end event can be processed.</returns>
        /// <remarks>
        /// This function was only made to make sure this interface wasn't empty. <br/>
        /// It is highly likely that this function/interface will be updated in the future.
        /// </remarks>
        bool IsValid();
    }
}
