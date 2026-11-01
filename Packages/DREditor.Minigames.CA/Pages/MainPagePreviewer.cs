// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PagePreviewer) +
        "\n\n")]
    public class MainPagePreviewer : PagePreviewer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int pageAddOnChildIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int overlayRendererIndex = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 focusPreviewScale = new(0.65f, 0.65f, 1);


        /// <inheritdoc/>
        public override void InitializePreviewPages(int currentPage)
        {
            base.InitializePreviewPages(currentPage);

            UpdatePreviewPage(currentPage - 1, focusPreviewScale, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="childIndex"></param>
        /// <param name="localScale"></param>
        /// <param name="overlayAlpha"></param>
        protected virtual void UpdatePreviewPage(int childIndex, Vector3 localScale, float overlayAlpha)
        {
            try
            {
                Transform toBepage = pages.transform.GetChild(childIndex);

                toBepage.transform.localScale = localScale;

                toBepage.GetChild(pageAddOnChildIndex)
                    .GetChild(overlayRendererIndex)
                    .GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 0, overlayAlpha);
            }
            catch
            {
                UnityEngine.Debug.LogWarning("ALERT: The preview page could not update the overlay, " +
                    "this usually means your indexes for values: pageAddOnChildIndex && overlayRendererIndex need " +
                    "to be updated!");
            }
        }
    }
}