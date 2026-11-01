//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IndexSelector) +
        "\n\n")]
    public class ScrollableIndexSelector : IndexSelector
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleLeft = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleRight = null;


        /// <inheritdoc/>
        public override void ActivateControls()
        {
            shuffleLeft.action.started += MoveLeft;

            shuffleRight.action.started += MoveRight;
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            shuffleLeft.action.started -= MoveLeft;

            shuffleRight.action.started -= MoveRight;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void MoveRight(CallbackContext ctx) => Move(selectionRate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void MoveLeft(CallbackContext ctx) => Move(-selectionRate);
    }
}