//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    public interface IFTEDataHandler : IPlayerDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        void AddCharacter(string name);


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetLevelOfCharacter(string name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        void LevelUpCharacter(string name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetAffinity(string name);
    }
}