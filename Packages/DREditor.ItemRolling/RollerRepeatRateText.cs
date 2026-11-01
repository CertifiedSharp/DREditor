//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.TextDisplaying;
using UnityEngine;

namespace DREditor.ItemRolling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DisplayFormattedString) +
        "\n\n")]
    public class RollerRepeatRateText : DisplayFormattedString
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BaseValueWithEvent paidAmount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useAssignedRoller;

        /// <summary>
        ///
        /// </summary>
        [ShowIf(nameof(UsingAssignedRoller))]
        [Tooltip("")]
        [SerializeField] protected ItemRollerBase assignableRoller;


        /// <summary>
        ///
        /// </summary>
        protected IItemRoller roller;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool UsingAssignedRoller() => useAssignedRoller;


        protected virtual void Start() => Display();

        protected virtual void OnEnable() => paidAmount.AddListener(OnUpdated);

        protected virtual void OnDisable() => paidAmount.RemoveListener(OnUpdated);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnUpdated() => Display();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override object GetObjectToBeFormatted() =>
            GetRoller() is IRepeatableRoller repeatable ? repeatable.GetRepeatRate() : -1;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IItemRoller GetRoller() => UsingAssignedRoller() ? GetAssignedRoller() : GetParentRoller();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IItemRoller GetAssignedRoller() => assignableRoller != null ? assignableRoller : GetParentRoller();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IItemRoller GetParentRoller() => roller ??= GetComponentInParent<IRollerContainer>().GetRoller();
    }
}