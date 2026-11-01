// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(IPageListener) +
        "\n\n")]
    public class PageArrows : ControlledComponent, IPageListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject leftArrow;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject rightArrow;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool inactiveOnInputDisabled = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string triggerString = "Click";


        /// <summary>
        ///
        /// </summary>
        protected int currentPage = 0;

        /// <summary>
        ///
        /// </summary>
        protected int previousPage = 0;

        /// <summary>
        ///
        /// </summary>
        protected int totalPages = 0;


        /// <inheritdoc/>
        public override void ActivateControls() => OnPageUpdated(currentPage, totalPages);

        /// <inheritdoc/>
        public override void DeactivateControls() => OnInputDisabled();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInputDisabled()
        {
            if (inactiveOnInputDisabled)
            {
                leftArrow.SetActive(false);

                rightArrow.SetActive(false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        public virtual void OnPageUpdated(int currentPage, int totalPages)
        {
            if (this.currentPage == currentPage)
            {
                this.currentPage = currentPage;

                this.totalPages = totalPages;

                return;
            }

            previousPage = this.currentPage;

            this.currentPage = currentPage;

            this.totalPages = totalPages;

            bool moreThanOnePage = totalPages > 1;

            bool notOnFirst = currentPage > 0;

            bool notOnLast = currentPage < totalPages - 1;

            leftArrow.SetActive(moreThanOnePage && notOnFirst);

            rightArrow.SetActive(moreThanOnePage && notOnLast);

            UpdateAnimation();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateAnimation()
        {
            bool movingRight = currentPage > previousPage;

            var selectedArrow = movingRight ? rightArrow : leftArrow;

            if (selectedArrow.TryGetComponent(out Animator animator) && selectedArrow.activeInHierarchy)
                animator.SetTrigger(triggerString);
        }
    }
}