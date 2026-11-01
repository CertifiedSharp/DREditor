//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    public interface ISubsequentConvert
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="genericSubsequents"></param>
        /// <param name="genericSubsequent"></param>
        void Convert(List<GenericSubsequent> genericSubsequents, GenericSubsequent genericSubsequent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericSubsequent"></param>
        /// <returns></returns>
        bool CanConvert(GenericSubsequent genericSubsequent);
    }
}