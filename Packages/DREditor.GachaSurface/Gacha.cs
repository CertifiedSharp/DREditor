//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FreeTimeSurfacing;
using DREditor.ItemRolling;
using DREditor.Processables;
using DREditor.Utility;
using UnityEngine;

namespace DREditor.GachaSurface
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SubmittableFTESurfaceCollection) + ", " + nameof(SubmittableFTESurfaceComponent) + ", " + 
        nameof(FTESurfaceComponent) + ", " + nameof(ISubmittable) + ", " + nameof(IComponentsCache) + ", " + 
        nameof(IRollerContainer) +
        "\n\n")]
    public class Gacha : SubmittableFTESurfaceCollection, IRollerContainer
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ItemRollerBase roller;


        /// <inheritdoc/>
        public virtual IItemRoller GetRoller() => roller;


        /// <inheritdoc/>
        protected override void OnSubmit()
        {
            RollItem();

            ProcessRollResult();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RollItem() => roller.RollForItem();

        /// <summary>
        ///
        /// </summary>
        protected virtual void ProcessRollResult()
        {
            if (RollSucceded())
                OnRollSucceded();
            else
                OnRollFailed();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool RollSucceded() => roller.TryGetRolledObject(out object _);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRollSucceded()
        {
            this.InvokeThisPlusComponents<IItemRollerResponse>((response) => response?.InitializeForResponse(roller));

            base.OnSubmit();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRollFailed()
        {
            AlertRollFailed();

            base.InvokeCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AlertRollFailed() =>
            UnityEngine.Debug.LogWarning($"ALERT: The item roller failed in rolling an item! Forcing process as Complete.");

        /// <summary>
        ///
        /// </summary>
        protected override void InvokeCompleted()
        {
            roller.GivePlayerRolledItem();

            base.InvokeCompleted();
        }
    }
}