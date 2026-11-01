//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using DREditor.Processables;
using System.Collections.Generic;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    public interface IRegulationsUIRepository : IEventInitializable
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<IRegulation> GetRegulations();
    }
}
