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
    public class EnvironmentSFXLineHandler : AudioLineHandler
    {
        /// <inheritdoc/>
        protected override void PlayLineAudio(Line currentLine)
            => ServiceLocator.GetService<EnvironmentSFXManangerBase>().PlayEnv(currentLine.EnvSFX);
    }
}