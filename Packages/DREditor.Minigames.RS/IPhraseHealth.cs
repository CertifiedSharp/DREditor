//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface IPhraseHealth
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="initialHealth"></param>
        void SetInitialHealth(int initialHealth);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        int GetInitialHealth();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        int GetHealth();

        /// <summary>
        ///
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        int TakeDamage(int damage);
    }
}