//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.CustomSettings
{
    /// <summary>
    ///
    /// </summary>
    public interface ICustomGameSettingsHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        CustomGameSettings Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="customSettings"></param>
        void Load(List<CustomGameSettings> customSettings);
    }
}