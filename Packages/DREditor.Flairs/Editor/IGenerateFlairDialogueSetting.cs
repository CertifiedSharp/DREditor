//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Collections.Generic;

namespace DREditor.Flairs.Editor
{
    /// <summary>
    ///
    /// </summary>
    public interface IGenerateFlairDialogueSetting
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="events"></param>
        void ExecuteOption(List<IFlairEvent> events);
    }
}
#endif