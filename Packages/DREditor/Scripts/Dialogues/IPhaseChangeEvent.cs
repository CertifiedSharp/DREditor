//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhaseChangeEvent<T>
    {
        /// <summary>
        ///
        /// </summary>
        event Action<T> PhaseChanged;
    }
}