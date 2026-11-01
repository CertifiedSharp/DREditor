//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhraseHandlerComponent
    {
        /// <summary>
        ///
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="phrase"></param>
        void StartPhrase<T>(T phrase) where T : PhraseBase;
    }
}