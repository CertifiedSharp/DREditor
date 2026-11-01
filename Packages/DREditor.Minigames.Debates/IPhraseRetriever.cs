//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhraseRetriever
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="phraseText"></param>
        /// <returns></returns>
        bool TryGetAimedPhrase(out PhraseTextBase phraseText);
    }
}