//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    public interface IModeVisibilityEntry
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="setVisibility"></param>
        void Initialize(Action<bool> setVisibility);

        /// <summary>
        ///
        /// </summary>
        void Resolve();
    }
}