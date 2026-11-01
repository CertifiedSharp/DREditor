//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Services;

namespace DREditor.AudioLineHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineEndProcess) +
        "\n\n")]
    public class LineEndStopVoiceLine : DialogueComponent, ILineEndProcess
    {
        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null) 
            => ServiceLocator.GetService<VOManagerBase>().StopVoiceLine();
    }
}