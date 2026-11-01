//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using UnityEngine.Events;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    public interface IFlairDestroyable
    {
        /// <summary>
        ///
        /// </summary>
        UnityEvent<IFlairDestroyable> OnFlairDestroyed { get; }
    }
}