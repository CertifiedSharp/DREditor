//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using System.Collections;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CGEventComponent<Flashback>) + " for the CG Style Dialogue Event: " + nameof(Flashback) +
        "\n\n")]
    public class CGFlashbackEvent : CGEventComponent<Flashback>, ILineProcessorRoutine
    {
        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine) => currentLine.ContainsEventType<Flashback>();

        /// <inheritdoc/>
        public virtual IEnumerator LineProcessRoutine(Line currentLine) => TriggerCGEvent(currentLine);
    }
}