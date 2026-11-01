//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using DREditor.UIHandling;
using UnityEngine;

namespace DREditor.TwoPointFiveD
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ITPFDListener) +
        "\n\n")]
    public class CameraArrow : MonoBehaviour, ITPFDListener
    {
        /// <summary>
        ///
        /// </summary>
        enum Direction
        {
            Up, Down, Left, Right
        }

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent arrow = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent cancel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Direction direction;


        /// <summary>
        ///
        /// </summary>
        protected bool inCancel = false;

        /// <summary>
        ///
        /// </summary>
        protected bool canChange = false;

        /// <summary>
        ///
        /// </summary>
        protected UIVisibilityComponent current;


        protected virtual void Start()
        {
            current = arrow;

            current.Show();
        }


        /// <inheritdoc/>
        public virtual void OnEnteredBoundary(int directionIndex)
        {
            if (!inCancel && directionIndex == (int)direction)
            {
                inCancel = true;

                if (!inTPFD.Value)
                    SetCanChange(false);
                else
                    ChangeArrowVisual(cancel, arrow);
            }
        }

        /// <inheritdoc/>
        public virtual void OnExitedBoundary(int directionIndex)
        {
            if (inCancel && directionIndex == (int)direction)
            {
                inCancel = false;

                if (!inTPFD.Value)
                    SetCanChange(false);
                else
                    ChangeArrowVisual(arrow, cancel);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        protected virtual void ChangeArrowVisual(UIVisibilityComponent to, UIVisibilityComponent from)
        {
            if (!canChange)
                return;
#if DOTWEEN
            to.DOKill();
            from.DOKill();
#endif
            to.Show();
            from.Hide();

            current = to;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="canChange"></param>
        public virtual void SetCanChange(bool canChange) => this.canChange = canChange;
    }
}