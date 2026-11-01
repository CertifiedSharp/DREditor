//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Processables
{
    /// <summary>
    ///
    /// </summary>
    public interface IBuildableDescription
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="baseDescription"></param>
        /// <param name="collectedFragments"></param>
        /// <returns></returns>
        string this[string baseDescription = "", List<string> collectedFragments = null] { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string this[int index] { get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string[] GetFragmentNames();
    }
}