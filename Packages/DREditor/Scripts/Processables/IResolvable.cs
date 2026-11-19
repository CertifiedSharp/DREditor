//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for objects or processes that can be resolved or settled.
    /// </summary>
    public interface IResolvable
    {
        /// <summary>
        /// Settles this systems process or state.
        /// </summary>
        void Resolve();
    }
}