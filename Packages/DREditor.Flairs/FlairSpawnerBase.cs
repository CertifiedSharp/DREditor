//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FlairSpawnerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInstance"></param>
        public abstract void Spawn(IFlairEvent eventInstance);
    }
}