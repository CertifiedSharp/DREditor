//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public interface IFlairSpawnStep
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInstance"></param>
        /// <returns></returns>
        GameObject SpawnFlair(IFlairEvent eventInstance);
    }
}