//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Dialogues.LineReading
{
    /// <summary>
    ///
    /// </summary>
    public interface ILineRoutine
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        bool ShouldProcess(Line currentLine);
    }
}