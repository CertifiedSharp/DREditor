//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor;
using DREditor.EventObjectServices;
using TMPro;
using UnityEngine;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IntWithEventListener) + ", " + nameof(IDigitPlace) + 
        "\n\n")]
    public class DisplayDigit : IntWithEventListener, IDigitPlace
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text digitText;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int digitPlace = 1;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 9)]
        [Tooltip("")]
        [SerializeField] protected int maxDigit = 9;


        /// <summary>
        ///
        /// </summary>
        private const int DigitModulus = 10;


        protected virtual void Awake()
        {
            if (DigitPlaceInvalid())
                UnityEngine.Debug.LogError("ALERT: THE DIGIT PLACE IS NOT VALID! Valid Examples: 1, 10, 100");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DigitPlaceInvalid() => digitPlace == 0 || (digitPlace > 1 && digitPlace % DigitModulus != 0);


        /// <inheritdoc/>
        protected override void OnValueChanged(int updatedValue)
        {
            base.OnValueChanged(updatedValue);

            UpdateDisplayedDigit();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateDisplayedDigit() => digitText.text = CalculateDigit().ToString();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int CalculateDigit() => UpdatedValue < digitPlace ? 0 : UpdatedValue / digitPlace % DigitModulus;


        /// <inheritdoc/>
        public virtual int GetDigitPlace() => digitPlace;

        /// <inheritdoc/>
        public virtual string GetDisplayedDigit() => digitText.text;

        /// <inheritdoc/>
        public virtual bool DisplayingMaxDigit() => GetDisplayedDigit() == maxDigit.ToString();
    }
}