// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPageListener) + ", " + nameof(IClosingArgumentListener) +
        "\n\n")]
    public class CorrectMiniPanelUpdater : MonoBehaviour, IPageListener, IClosingArgumentListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pagebar;


        /// <summary>
        ///
        /// </summary>
        protected int currentPage = 0;


        /// <inheritdoc/>
        public virtual void OnPageUpdated(int currentPage, int totalPages) => this.currentPage = currentPage;


        /// <inheritdoc/>
        public virtual void OnStockCorrect(Transform hoveredObject)
        {
            Transform pageBarPanel = pagebar.transform
                .GetChild(currentPage)
                .GetChild(hoveredObject.GetSiblingIndex() - 1);

            if (pageBarPanel.TryGetComponent(out QuestionPanelBase miniPanel))
                miniPanel.OnPanelCorrect();
        }


        /// <inheritdoc/>
        public virtual void CAReset() => currentPage = 0;
    }
}