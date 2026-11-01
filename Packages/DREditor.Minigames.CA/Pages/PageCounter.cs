// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPageListener) +
        "\n\n")]
    public class PageCounter : MonoBehaviour, IPageListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text pageCounter;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color32 pageCounterColor = new(227, 209, 62, 255);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string pageTagStart = "<color=#E3D13E>";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string pageTagEnd = "</color>";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string pageDividerString = "<size=70%> /";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int currentPageOffset = 1;


        /// <inheritdoc/>
        public virtual void OnPageUpdated(int currentPage, int totalPages)
        {
            int pageNumber = currentPage + currentPageOffset;

            pageCounter.text = pageTagStart + pageNumber + pageTagEnd + pageDividerString + totalPages;

            var children = pageCounter.GetComponentsInChildren<TMP_Text>();

            foreach (var child in children)
                if (child != pageCounter)
                    child.text = pageNumber + pageDividerString + totalPages;
        }
    }
}