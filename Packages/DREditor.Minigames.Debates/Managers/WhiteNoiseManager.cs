//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhraseManagerBase) + ", " + nameof(IPhraseManager) +
        "\n\n")]
    public class WhiteNoiseManager : PhraseManagerBase
    {
        /// <inheritdoc/>
        public override float PlayPhrases(Panel panel)
        {
            activePhrases.Clear();

            int phraseTextIndex = 0;

            for (int i = 0; i < panel.Phrases.Count; i++)
                if (panel.Phrases[i] is WhiteNoise whiteNoise)
                {
                    float duration = phraseTexts[phraseTextIndex].Initialize(whiteNoise);

                    OnPhraseInitialized?.Invoke(panel, phraseTexts[phraseTextIndex], duration);

                    phraseTextIndex++;

                    activePhrases.Add(phraseTexts[phraseTextIndex]);
                }

            return 0;
        }
    }
}