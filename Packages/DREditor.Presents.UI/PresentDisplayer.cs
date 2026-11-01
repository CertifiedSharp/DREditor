//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using DREditor.UI;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PresentDisplayer : SelectedItemDisplayer
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI percent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI id = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI named = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI amount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI description = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string amountPrefix = "X ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string amountFormat = "D3";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string idPrefix = "NO. ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string unknownDataText = "???";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string percentFormat = "000";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool roundPercent = true;


        /// <inheritdoc/>
        public override void DisplaySelectedItem(int index)
        {
            Present present = presentDatabase.presents[index];

            IAmountablePresent presentData = ServiceLocator.GetService<IPresentManager>().Get<IAmountablePresent>(present.ID);

            percent.text = GetPercent(ServiceLocator.GetService<IPresentManager>().Count, presentDatabase.presents.Count);

            id.text = GetFormattedAmount(idPrefix, present.ID, amountFormat);

            if (presentData != null)
            {
                amount.text = GetFormattedAmount(amountPrefix, presentData.Amount, amountFormat);

                named.text = present.DisplayName;

                description.text = present.Description;
            }
            else
            {
                amount.text = unknownDataText;

                named.text = unknownDataText;

                description.text = unknownDataText;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected virtual string GetFormattedAmount(string prefix, int value, string format)
            => $"{prefix}{value.ToString(format)}";

        /// <summary>
        ///
        /// </summary>
        /// <param name="part"></param>
        /// <param name="whole"></param>
        /// <returns></returns>
        protected virtual string GetPercent(int part, int whole)
        {
            float percent;

            percent = (float)((double)part / whole * 100);

            if (roundPercent)
                percent = (float)Mathf.Round((float)percent);

            return percent.ToString(percentFormat, CultureInfo.InvariantCulture) + " %";
        }
    }
}