//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.ItemRolling;
using DREditor.Presents;
using DREditor.Processables;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.GachaSurface
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RepeatableRollerBase) + ", " + nameof(ItemRollerBase) + ", " + 
        nameof(IItemRoller) + ", " + nameof(IRepeatableRoller) +
        "\n\n")]
    public class PresentRoller : RepeatableRollerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected IntWithEvent paidAmount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;


        /// <summary>
        ///
        /// </summary>
        protected Present rolledObject;

        /// <summary>
        ///
        /// </summary>
        protected IPresentManager presentManager;


        /// <summary>
        ///
        /// </summary>
        protected readonly float OneHundred = 100;

        /// <summary>
        ///
        /// </summary>
        protected readonly float AmountToPercentScale = 10;

        /// <summary>
        ///
        /// </summary>
        protected readonly int Zero = 0;

        /// <summary>
        ///
        /// </summary>
        protected readonly int InvalidRepeatRate = -1;


        /// <inheritdoc/>
        protected override object GetRolledObject() => rolledObject;

        /// <inheritdoc/>
        protected override void AddItemToInventory() => presentManager.AddPresent(rolledObject.ID);


        /// <inheritdoc/>
        public override object RollForItem(params object[] objects) =>
            rolledObject = ServicesMissing() ? 
            null : PaidValidAmount() ? 
            RollPresent() : null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ServicesMissing()
        {
            bool databaseMissing = presentDatabase == null;

            bool presentManagerMissing = !ServiceLocator.HasService(out presentManager);

            if (presentManagerMissing)
                UnityEngine.Debug.LogError($"ALERT: Repeat Amount calculation failed because there is currently no service " +
                    $"matching type {nameof(IPresentManager)} please add one to the scene.");

            if (databaseMissing)
                UnityEngine.Debug.LogError($"ALERT: The Present Database is missing! Please assign one to this present roller.");

            return presentManagerMissing || databaseMissing;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool PaidValidAmount()
        {
            bool result = paidAmount.Value > Zero;

            if (!result)
                OnAmountPaidInvalid();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnAmountPaidInvalid() =>
            UnityEngine.Debug.LogError($"ALERT: Item Roller {nameof(PresentRoller)} was not given proper payment!");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Present RollPresent() =>
            RollPresentFrom(presentDatabase.presents) is not Present present ?
            null : PlayerOwnsItem(present) && ShouldGetNewPresent() ?
            RollUnownedPresent(present) : present;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Present RollPresentFrom(List<Present> collection) =>
            (Present)collection[UnityEngine.Random.Range(Zero, collection.Count)];

        /// <inheritdoc/>
        protected override bool PlayerOwnsItem(object item) =>
            item is IIdentifiable present && presentManager.HadPresent(present.ID);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldGetNewPresent() =>
            GetRepeatRate() is float modifiedRepeatRate &&
            modifiedRepeatRate != OneHundred &&
            SuccededChanceForNewPresent(modifiedRepeatRate);


        /// <inheritdoc/>
        public override float GetRepeatRate()
        {
            if (ServicesMissing())
                return InvalidRepeatRate;
            else if (PlayerHasAllPresents())
                return OneHundred;
            else
                return CalculateRepeatRateByPaidAmount();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayerHasAllPresents() => presentManager.Count == presentDatabase.presents.Count;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateRepeatRateByPaidAmount()
        {
            float repeatAmount = base.GetRepeatRate() - (paidAmount.Value / AmountToPercentScale);

            return repeatAmount < Zero ? Zero : repeatAmount;
        }

        /// <inheritdoc/>
        protected override float CalculateBaseRepeatRate() =>
            presentManager.Count * OneHundred / presentDatabase.presents.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="modifiedRepeatRate"></param>
        /// <returns></returns>
        protected virtual bool SuccededChanceForNewPresent(float modifiedRepeatRate)
        {
            float trueRepeatRate = presentManager.Count / (float)presentDatabase.presents.Count;

            float scaledRepeatRate = trueRepeatRate > Zero ? modifiedRepeatRate / trueRepeatRate : OneHundred;

            if (scaledRepeatRate > OneHundred)
                scaledRepeatRate = OneHundred;

            return UnityEngine.Random.Range(Zero, (int)OneHundred) > scaledRepeatRate;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="present"></param>
        /// <returns></returns>
        protected virtual Present RollUnownedPresent(Present present) =>
            TryRollUnownedPresent(out Present unownedPresent) ? unownedPresent : present;

        /// <summary>
        ///
        /// </summary>
        /// <param name="present"></param>
        /// <returns></returns>
        protected virtual bool TryRollUnownedPresent(out Present present) =>
            present = ServicesMissing() ? null : RollUnownedPresent();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Present RollUnownedPresent()
        {
            List<Present> unownedPresents = presentDatabase.presents
                .Where(present => present != null && !PlayerOwnsItem(present))
                .ToList();

            bool allPresentsOwned = unownedPresents.Count == Zero;

            return allPresentsOwned ? RollPresentFrom(presentDatabase.presents) : RollPresentFrom(unownedPresents);
        }
    }
}