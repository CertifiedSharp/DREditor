//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using DREditor.Nameplates;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueNameplateHandler) + "\n\n")]
    public class TrialPortraitHandler : DialogueNameplateHandler
    {
        /// <inheritdoc/>
        public override void SetNameplate(Line currentLine)
        {
            if (currentLine.AliasNumber != 0)
                SetTexture(currentLine.Speaker.Aliases[currentLine.AliasNumber - 1].TrialPortrait);
            else if (currentLine.Speaker != null && currentLine.Speaker.TrialPortrait != null)
                SetTexture(currentLine.Speaker.TrialPortrait);
        }
    }
}