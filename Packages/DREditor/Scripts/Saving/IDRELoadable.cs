//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    public interface IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        void Load(Dictionary<string, object> saveDatas);
    }
}