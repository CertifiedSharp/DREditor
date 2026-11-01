//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Services;
using DREditor.TextDisplaying;

namespace DREditor.Backlogging
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ILineEndProcess) + 
        "\n\n")]
    public class BacklogLineHandler : DialogueComponent, ILineEndProcess
    {
        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine) => AddLineToBacklog(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void AddLineToBacklog(Line currentLine)
        {
            if (currentLine.Speaker != null)
                ServiceLocator.GetService<BacklogHandlerBase>().AddLine(CreateBacklogLine(currentLine));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        protected virtual BacklogLines CreateBacklogLine(Line currentLine) =>
            new(
                currentLine.Speaker,
                ServiceLocator.GetService<ITextDisplayerHandler>().GetTextBox().text,
                currentLine.AliasNumber - 1,
#if FMOD_UNITY
                currentLine.VoiceSFX,
#else
                currentLine.VoiceSFX.name,
#endif
                ServiceLocator.GetService<ITextDisplayerHandler>().GetTextBox().color
                );
    }
}