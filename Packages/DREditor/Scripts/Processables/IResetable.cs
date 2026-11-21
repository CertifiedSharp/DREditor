//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for processes that can be reset, or can be started again.
    /// </summary>
    /// <remarks>It should be noted that this is different than Unity's Reset() function where settings are reset.</remarks>
    public interface IResetable
    {
        /// <summary>
        /// The reset process for this script. (Not to be confused with Unity's Reset() function)
        /// </summary>
        void OnReset();
    }
}