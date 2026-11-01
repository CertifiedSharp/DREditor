//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    public interface ICGHandler
    {
        /// <summary>
        ///
        /// </summary>
        void TransitionOffState();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        BoolWithEvent InCGStateMode();
    }
}