//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.UIHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityHandler) +
        "\n\n")]
    public class ToggleTextVisibility : UIVisibilityHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text toggleText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string showText = " [F2] Show Question";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideText = " [F2] Hide Question";


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => toggleText.text = showText;


        /// <summary>
        ///
        /// </summary>
        public virtual void Toggle() => StartCoroutine(ToggleTextRoutine());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ToggleTextRoutine()
        {
            yield return uiComponent.HideTillComplete();

            toggleText.text = toggleText.text == showText ? hideText : showText;

            yield return uiComponent.ShowTillComplete();

            yield break;
        }
    }
}