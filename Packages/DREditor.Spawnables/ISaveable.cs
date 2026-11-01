//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Spawnables
{
    /// <summary>
    ///
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        int SubID { get; }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        void Load(string data);
    }
}