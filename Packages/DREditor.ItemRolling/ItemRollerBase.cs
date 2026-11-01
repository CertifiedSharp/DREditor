//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.ItemRolling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ItemRollerBase : MonoBehaviour, IItemRoller
    {
        /// <inheritdoc/>
        public virtual bool TryGetRolledObject<T>(out T rolledObject)
        {
            bool result = (rolledObject = GetRolledObject() is T resultObject ? resultObject : default(T)) != null;

            return ReportElse(result,
                $"ALERT: The request to get the rolled item failed because it was not of the expected type!" +
                $"\nANALYSIS: Expected type: {typeof(T).Name} : Got type: {GetRolledObject().GetType().Name}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool ReportElse(bool condition, string message)
        {
            if (!condition)
                Debug.Log(message);

            return condition;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract object GetRolledObject();

        /// <inheritdoc/>
        public abstract object RollForItem(params object[] objects);

        /// <inheritdoc/>
        public abstract void GivePlayerRolledItem();
    }
}