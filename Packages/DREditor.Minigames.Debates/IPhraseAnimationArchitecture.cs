//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhraseAnimationArchitecture
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        void Initialize(object o = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="architecture"></param>
        void Apply(IPhraseAnimationArchitecture architecture);
    }
}