//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public interface IFlairStep
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInstance"></param>
        /// <param name="spawnedFlair"></param>
        void ApplyStep(IFlairEvent eventInstance, GameObject spawnedFlair);
    }
}