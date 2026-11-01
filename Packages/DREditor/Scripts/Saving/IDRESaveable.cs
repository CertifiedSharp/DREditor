//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
namespace DREditor.Saving
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public interface IDRESaveable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        void Save(Dictionary<string, object> saveDatas);
    }

}