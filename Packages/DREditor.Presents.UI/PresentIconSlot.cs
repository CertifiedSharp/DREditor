//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.Services;
using DREditor.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IGridIconSlot) +
        "\n\n")]
    public class PresentIconSlot : MonoBehaviour, IGridIconSlot
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image presentIcon = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite undiscoveredIcon;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject newIcon = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Color selectedColor = Color.white;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color unselectedColor = Color.gray;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI amountText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string amountPrefix = "X ";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string amountFormat = "D3";


        /// <inheritdoc/>
        public virtual void UpdateSlot(int globalIndex, bool selected)
        {
            Present entry = presentDatabase.presents[globalIndex];

            var pres = ServiceLocator.GetService<IPresentManager>().Get<IIdentifiable>(entry.ID);

            if (selected && pres is INewPresentObtained obtainable && obtainable.JustObtained)
                obtainable.NoLongerNew();

            presentIcon.gameObject.SetActive(true);

            if (selected)
                presentIcon.color = selectedColor;
            else
                presentIcon.color = unselectedColor;

            UpdateIcon(entry, pres);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="presentData"></param>
        protected virtual void UpdateIcon(Present entry, IIdentifiable presentData)
        {
            if (presentData != null)
            {
                presentIcon.sprite = entry.Image;

                if (newIcon != null && presentData is INewPresentObtained obtainable)
                    newIcon.SetActive(obtainable.JustObtained);

                if (presentData is IAmountablePresent amountable)
                    amountText.text = $"{amountPrefix} {amountable.Amount.ToString(amountFormat)}";
            }
            else
            {
                presentIcon.sprite = undiscoveredIcon;

                if (newIcon != null)
                    newIcon.SetActive(false);

                amountText.text = string.Empty;
            }
        }


        /// <inheritdoc/>
        public virtual void ClearSlotState()
        {
            presentIcon.gameObject.SetActive(false);

            presentIcon.sprite = undiscoveredIcon;

            if (newIcon != null)
                newIcon.SetActive(false);

            amountText.text = string.Empty;
        }
    }
}