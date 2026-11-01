//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DisplayFormattedString : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected TMP_Text display;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string prefix = "x";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string format = "{0:000}";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string suffix = "";


        protected virtual void Awake()
        {
            if (display == null)
                UnityEngine.Debug.LogError($"ALERT: The {nameof(display)} field must be assigned to!");
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void Display() => display.SetText(CreateDisplayString());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateDisplayString() => prefix + CreateFormattedString() + suffix;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateFormattedString() => string.Format(format, GetObjectToBeFormatted());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract object GetObjectToBeFormatted();
    }
}