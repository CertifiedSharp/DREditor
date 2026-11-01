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
    public class MusicLineHandler : AudioLineHandler
    {
        /// <inheritdoc/>
        protected override void PlayLineAudio(Line currentLine)
        {
            if (currentLine.MusicChange)
                ServiceLocator.GetService<MusicManagerBase>().PlayMusic(currentLine.Music);
        }
    }
}