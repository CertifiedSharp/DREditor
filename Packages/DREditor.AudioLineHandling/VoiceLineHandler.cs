//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Dialogues;
using DREditor.Services;

namespace DREditor.AudioLineHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AudioLineHandler) + ", " + nameof(DialogueComponent) +
        "\n\n")]
    public class VoiceLineHandler : AudioLineHandler
    {
        /// <inheritdoc/>
        protected override void PlayLineAudio(Line currentLine)
            => ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(currentLine.VoiceSFX);
    }
}