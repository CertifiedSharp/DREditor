//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="saveDatas"></param>
        /// <param name="inChapterSelect"></param>
        void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect);
    }
}