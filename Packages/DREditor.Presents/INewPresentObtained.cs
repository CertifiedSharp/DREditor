//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    public interface INewPresentObtained : IIdentifiable
    {
        /// <summary>
        ///
        /// </summary>
        public bool JustObtained { get; set; }


        /// <summary>
        ///
        /// </summary>
        void NoLongerNew();
    }
}