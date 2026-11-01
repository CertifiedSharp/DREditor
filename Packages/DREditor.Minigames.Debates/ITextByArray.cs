//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface ITextByArray
    {
        /// <summary>
        ///
        /// </summary>
        int WordCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        int WordStart { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Text { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        void SetText(string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        void SetStart(int start);

        /// <summary>
        ///
        /// </summary>
        /// <param name="count"></param>
        void SetCount(int count);
    }
}