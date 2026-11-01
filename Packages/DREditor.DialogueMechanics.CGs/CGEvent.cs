//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using System;
using System.Collections;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CGEventComponent<CGDisplay>) + " for the CG Style Dialogue Event: " + nameof(CGDisplay) + 
        "\n\n")]
    public class CGEvent : CGEventComponent<CGDisplay>, ILineLastingEvent
    {
        /// <inheritdoc/>
        public virtual IEnumerator LineLastingEvaluation(Line currentLine, Action continueLineCallback = null)
            => TriggerCGEvent(currentLine);
    }
}