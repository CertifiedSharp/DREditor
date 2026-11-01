//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    public interface ILoadOnChapterSelect
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="oldSaveDatas"></param>
        /// <param name="newSaveDatas"></param>
        void CarryOverOnChapterSelect(Dictionary<string, object> oldSaveDatas, Dictionary<string, object> newSaveDatas);
    }
}