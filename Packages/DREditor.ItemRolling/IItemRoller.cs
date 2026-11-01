//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.ItemRolling
{
    /// <summary>
    ///
    /// </summary>
    public interface IItemRoller
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        object RollForItem(params object[] objects);

        /// <summary>
        ///
        /// </summary>
        void GivePlayerRolledItem();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rolledObject"></param>
        /// <returns></returns>
        bool TryGetRolledObject<T>(out T rolledObject);
    }
}