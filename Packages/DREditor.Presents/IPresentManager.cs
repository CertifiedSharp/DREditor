//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    public interface IPresentManager
    {
        /// <summary>
        ///
        /// </summary>
        int Count { get; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(int id) where T : IIdentifiable;

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amt"></param>
        void AddPresent(int id, int amt = 1);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool HadPresent(int id);
    }
}