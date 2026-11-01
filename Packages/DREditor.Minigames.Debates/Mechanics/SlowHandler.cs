//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates.Mechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SpeedHandlerBase) + ", " + nameof(ControlledComponent) + ", " + nameof(IPanelsResolvedListener) + ", " +
        nameof(IPanelsStoppedListener) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class SlowHandler : SpeedHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference focus = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasStamina = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isDrainingStaminia = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected VolumeProfile volumeProfile = null;


        /// <summary>
        ///
        /// </summary>
        protected Volume minigameVolume = null;


        protected override event Action<CallbackContext> StartInput
        {
            add => focus.action.started += value;

            remove => focus.action.started -= value;
        }

        protected override event Action<CallbackContext> StopInput
        {
            add => focus.action.canceled += value;

            remove => focus.action.canceled -= value;
        }


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            if (volumeProfile != null)
                minigameVolume = FindObjectsByType<Volume>()
                .Where(foundVolume => foundVolume.profile.name == volumeProfile.name)
                .FirstOrDefault();
        }


        /// <inheritdoc/>
        public override void ActivateControls()
        {
            base.ActivateControls();

            if (hasStamina != null)
                hasStamina.AddListener(OnStaminaUpdated);
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            base.DeactivateControls();

            if (hasStamina != null)
                hasStamina.AddListener(OnStaminaUpdated);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hasStamina"></param>
        protected virtual void OnStaminaUpdated(bool hasStamina)
        {
            if (!hasStamina)
                StopSpeedChange(new());
        }

        /// <inheritdoc/>
        protected override bool SpeedChangeShouldStart() => (hasStamina == null || hasStamina.Value) && base.SpeedChangeShouldStart();

        /// <inheritdoc/>
        protected override void SetChangingSpeedState(bool inSpeedState)
        {
            base.SetChangingSpeedState(inSpeedState);

            SetDrainingStamina(inSpeedState);

            if (minigameVolume != null)
                minigameVolume.enabled = !inSpeedState;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="drainingStamina"></param>
        protected virtual void SetDrainingStamina(bool drainingStamina) => isDrainingStaminia.SetValue(drainingStamina);


        /// <inheritdoc/>
        protected override void ToggleVelocity(bool mode)
        {
            SetDrainingStamina(mode);

            base.ToggleVelocity(mode);
        }


        /// <inheritdoc/>
        public override void OnPanelsResolved()
        {
            SetDrainingStamina(false);

            base.OnPanelsResolved();
        }
    }
}