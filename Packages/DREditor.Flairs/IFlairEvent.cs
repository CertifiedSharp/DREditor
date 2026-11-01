//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public interface IFlairEvent
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IFlair GetFlair();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<IFlairSetting> GetSettings();

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnedFlair"></param>
        void ApplySettings(GameObject spawnedFlair);
    }
}