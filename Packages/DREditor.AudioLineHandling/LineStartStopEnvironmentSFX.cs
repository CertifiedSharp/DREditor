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
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineStarter) +
        "\n\n")]
    public class LineStartStopEnvironmentSFX : DialogueComponent, ILineStarter
    {
        /// <inheritdoc/>
        public virtual void LineStartProcess(Line currentLine) => StopEnvironmentSFX(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void StopEnvironmentSFX(Line currentLine)
        {
            if (currentLine.StopEnv)
                ServiceLocator.GetService<EnvironmentSFXManangerBase>().StopEnv();
        }
    }
}