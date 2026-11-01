//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CurrencyHandler<T> : MonoBehaviour, IPlayerDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="amount"></param>
        public abstract void AddCurrency(T amount);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract T GetCurrency();

        /// <summary>
        ///
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public abstract bool CanPay(T amount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="amount"></param>
        public abstract void RemoveCurrency(T amount);

        /// <summary>
        ///
        /// </summary>
        public abstract void ClearData();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract bool IsBroke();
    }
}