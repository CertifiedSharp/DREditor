//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates.Absorbing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) +
        "\n\n")]
    public class AbsorbInputHandler : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference specialAction = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent absorbing = null;


        [Header("")]
        [SerializeField] protected UnityEvent absorbKeyPressed = null;


        [Header("")]
        [SerializeField] protected UnityEvent absorbKeyReleased = null;



        /// <inheritdoc/>
        public override void ActivateControls()
        {
            specialAction.action.started += StartAbsorbing;

            specialAction.action.canceled += EndAbsorbing;
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            specialAction.action.started -= StartAbsorbing;

            specialAction.action.canceled -= EndAbsorbing;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void StartAbsorbing(CallbackContext context)
        {
            absorbing.SetValue(true);

            absorbKeyPressed?.Invoke();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void EndAbsorbing(CallbackContext context)
        {
            absorbing.SetValue(false);

            absorbKeyReleased?.Invoke();
        }
    }
}