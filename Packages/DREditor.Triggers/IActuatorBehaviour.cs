//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    public interface IActuatorBehaviour : IEventTriggerSaver
    {
        /// <summary>
        ///
        /// </summary>
        void Call();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ISubsequentBehaviour[] GetSubsequents();
    }
}