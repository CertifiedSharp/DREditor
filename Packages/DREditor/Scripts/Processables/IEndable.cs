//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for a process that can be ended.
    /// </summary>
    public interface IEndable
    {
        /// <summary>
        /// The designated end process for this scripts core functionality.
        /// </summary>
        void EndProcess();
    }
}