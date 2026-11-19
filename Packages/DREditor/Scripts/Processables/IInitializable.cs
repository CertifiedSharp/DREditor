//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for objects or processes that can be initialized or have it's initial configuration set up.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Starts up this system's process or state.
        /// </summary>
        void Initialize();
    }
    // Was scrapped but keeping it here since it seemed like this had potential.
    ///// <summary></summary>
    ///// <typeparam name="T"></typeparam>
    //public interface IInitializable<T>
    //{
    //    /// <summary></summary>
    //    /// <param name="parameters"></param>
    //    void Initialize(params T[] parameters);
    //}
}