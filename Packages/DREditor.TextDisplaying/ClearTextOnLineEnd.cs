//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Services;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineEndProcess) +
        "\n\n")]
    public class ClearTextOnLineEnd : DialogueComponent, ILineEndProcess
    {
        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null) => ServiceLocator.GetService<ITextDisplayerHandler>().ClearText();
    }
}