//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for a process that can be started.
    /// </summary>
    public interface IStartable
    {
        /// <summary>
        /// The designated start process for this scripts core functionality.
        /// </summary>
        void StartProcess();
    }
}