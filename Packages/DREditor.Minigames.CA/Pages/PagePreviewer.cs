// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PageHandlerBase) + ", " + nameof(IClosingArgumentPagePreviewer) +
        "\n\n")]
    public class PagePreviewer : MonoBehaviour, IClosingArgumentPagePreviewer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pages;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 movePages = new(-2.793402478f, 0.192100486f, 0); //new (-9.887051203f, 0.6740315305f, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 pagesRotation = new(0, 0, -3.934f); //new(0, 0, -3.9f);


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPage"></param>
        public virtual void InitializePreviewPages(int currentPage)
        {
            ApplyPreviewPageTransform(pages.transform, movePages, currentPage);

            pages.transform.rotation = Quaternion.Euler(pagesRotation);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pagesTransform"></param>
        /// <param name="moveAmount"></param>
        /// <param name="currentPage"></param>
        protected virtual void ApplyPreviewPageTransform(Transform pagesTransform, Vector3 moveAmount, int currentPage)
        {
            pagesTransform.position += new Vector3(
                moveAmount.x * (currentPage - 1),
                moveAmount.y * (currentPage - 1),
                moveAmount.z);
        }
    }
}