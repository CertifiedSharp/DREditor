//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Investigations;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventManager) + ", " + nameof(DialogueComponent) + ", " + nameof(ILineEndProcess) +
        "\n\n")]
    public class TruthBulletGetOnLineEnd : DialogueEventManager, ILineEndProcess
    {
        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null) => TriggerAllDialogueEventsOfType<TruthBulletGet>(currentLine);
    }
}