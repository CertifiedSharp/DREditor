//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class PresentData : PresentSaveBase, INewPresentObtained, IAmountablePresent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int amount = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool justObtained = true;


        public virtual int Amount { get => amount; set => amount = value; }

        public virtual bool JustObtained { get => justObtained; set => justObtained = value; }


        public PresentData() { }

        public PresentData(int ID, int amount) : base(ID) => this.amount = amount;


        /// <inheritdoc/>
        public virtual void NoLongerNew() => JustObtained = false;

        /// <inheritdoc/>
        public virtual void Add(int amt = 1) => Amount += amt;

        /// <inheritdoc/>
        public virtual void Remove(int amt = 1)
        {
            if (Amount > 0)
                Amount -= amt;
            else
                UnityEngine.Debug.LogWarning("ALERT: You are trying to remove a present the player doesn't have enough of.");
        }
    }
}