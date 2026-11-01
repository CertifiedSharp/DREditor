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
    public class VectorIndexSelector : IndexSelector
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference navigate = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool xAxisInput = true;


        /// <inheritdoc/>
        public override void ActivateControls() => navigate.action.performed += Navigate;

        /// <inheritdoc/>
        public override void DeactivateControls() => navigate.action.performed -= Navigate;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void Navigate(CallbackContext ctx) => Move(selectionRate * GetAxis(ctx));

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual int GetAxis(CallbackContext input) =>
            (int)(xAxisInput ? input.ReadValue<Vector2>().x : input.ReadValue<Vector2>().y);
    }
}