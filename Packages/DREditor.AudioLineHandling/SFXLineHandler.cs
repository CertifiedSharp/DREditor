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
    public class SFXLineHandler : AudioLineHandler
    {
        /// <inheritdoc/>
        protected override void PlayLineAudio(Line currentLine)
        {
            if (currentLine.SFXs.Count > 0)
                for (int j = 0; j < currentLine.SFXs.Count; j++)
                    ServiceLocator.GetService<SFXManagerBase>().PlaySFX(currentLine.SFXs[j]);
        }
    }
}