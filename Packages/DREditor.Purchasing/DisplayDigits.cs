//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Purchasing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DisplayDigits : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference move = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool useHorizontalInput = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int inputDirectionalRate = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float deadThreshold = 0;


        /// <summary>
        ///
        /// </summary>
        protected int directionalRate = 0;

        /// <summary>
        ///
        /// </summary>
        protected CallbackContext context;

        /// <summary>
        ///
        /// </summary>
        protected readonly int Zero = 0;


        /// <inheritdoc/>
        public override void ActivateControls() => move.action.started += UpdateDisplayedCurrency;

        /// <inheritdoc/>
        public override void DeactivateControls() => move.action.started -= UpdateDisplayedCurrency;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void UpdateDisplayedCurrency(CallbackContext context)
        {
            InitializeOnInput(context);

            ProcessInputAmount();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void ProcessInputAmount();


        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void InitializeOnInput(CallbackContext context)
        {
            InitializeInputContext(context);

            InitializeDirectionalRate();

            InitializeSelectedDigit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void InitializeInputContext(CallbackContext context) => this.context = context;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDirectionalRate() => directionalRate = inputDirectionalRate * (int)CalculateDirection();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateDirection() => DirectionIsValid() ? Mathf.Sign(GetAxis()) : Zero;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DirectionIsValid() => InputPastDeadZone() && !MovingAlongSelectionAxis();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InputPastDeadZone() => Mathf.Abs(GetAxis()) > deadThreshold;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool MovingAlongSelectionAxis() => Mathf.Abs(GetOppositeAxis()) > Mathf.Abs(GetAxis());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float GetAxis() =>
            useHorizontalInput ? context.ReadValue<Vector2>().x : context.ReadValue<Vector2>().y;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float GetOppositeAxis() =>
            GetAxis() == context.ReadValue<Vector2>().x ? context.ReadValue<Vector2>().y : context.ReadValue<Vector2>().x;


        /// <summary>
        ///
        /// </summary>
        protected abstract void InitializeSelectedDigit();
    }
}