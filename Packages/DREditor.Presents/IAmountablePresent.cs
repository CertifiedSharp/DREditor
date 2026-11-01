//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    public interface IAmountablePresent : IIdentifiable
    {
        /// <summary>
        ///
        /// </summary>
        public int Amount { get; set; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="amt"></param>
        void Add(int amt = 1);

        /// <summary>
        ///
        /// </summary>
        /// <param name="amt"></param>
        void Remove(int amt = 1);
    }
}