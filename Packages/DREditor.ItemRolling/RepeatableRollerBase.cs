//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.ItemRolling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RepeatableRollerBase : ItemRollerBase, IRepeatableRoller
    {
        [Header("")]
        [Divider]
        [Header("")]
        [SerializeField] protected UnityEvent onRepeatPull;

        [Header("")]
        [SerializeField] protected UnityEvent onNewPull;


        /// <inheritdoc/>
        public virtual float GetRepeatRate() => CalculateBaseRepeatRate();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract float CalculateBaseRepeatRate();


        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract bool PlayerOwnsItem(object item);


        /// <inheritdoc/>
        public override void GivePlayerRolledItem()
        {
            if (GetRolledObject() == null)
                return;

            InvokePulledType();

            AddItemToInventory();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokePulledType()
        {
            if (PlayerOwnsItem(GetRolledObject()))
                onRepeatPull?.Invoke();
            else
                onNewPull?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void AddItemToInventory();
    }
}