//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
	[ClassHeader("")]
    public class ScrollOption : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image cell = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI text = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Selectable selectable = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        public virtual void EnableState(string title)
        {
            text.text = title;

            cell.enabled = true;

            selectable.interactable = true;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void DisableState()
        {
            text.text = "";

            cell.enabled = false;

            selectable.interactable = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Selectable GetSelectable() => selectable;
    }
}