//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;

namespace DREditor.Nameplates
{
    /// <summary>
    ///
    /// </summary>
    public class TrialDialogueNameplateHandler : DialogueNameplateHandler
    {
        /// <inheritdoc/>
        public override void SetNameplate(Line currentLine)
        {
            if (currentLine.AliasNumber != 0)
                SetTexture(currentLine.Speaker.Aliases[currentLine.AliasNumber - 1].TrialNameplate);
            else if (currentLine.Speaker != null && currentLine.Speaker.TrialNameplate != null)
                SetTexture(currentLine.Speaker.TrialNameplate);
        }
    }
}