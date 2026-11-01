//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.Input
{
    /// <summary>
    ///
    /// </summary>
    [DefaultExecutionOrder(-1)]
    [ClassHeader(nameof(ServiceHolder<IInputManager>) + " for type: " + nameof(IInputManager) +
        "\n\n")]
    public class PlayerInputHandler : ServiceHolder<IInputManager>, IInputManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionAsset controls = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference cancel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference leave = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugInputControls = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string keyboardScheme = "Keyboard&Mouse";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string gamepadScheme = "Gamepad";


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected DRControls Actions;

        /// <summary>
        ///
        /// </summary>
        protected PlayerInput playerInput;

        /// <summary>
        ///
        /// </summary>
        protected bool active = false;

        /// <summary>
        ///
        /// </summary>
        protected string currentScheme = "";

        /// <summary>
        ///
        /// </summary>
        protected readonly string NullName = "null";

        /// <summary>
        ///
        /// </summary>
        protected readonly string MouseName = "Mouse";

        /// <summary>
        ///
        /// </summary>
        protected readonly string SwitchName = "Switch";


        /// <inheritdoc/>
        protected override void Awake()
        {
            InitializePlayerInput();

            if (controls == null)
                UnityEngine.Debug.LogError($"ALERT: You MUST assign the {nameof(controls)} field!");

            base.Awake();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePlayerInput()
        {
            if (playerInput != null)
                playerInput.neverAutoSwitchControlSchemes = true;
            else
                playerInput = (PlayerInput)FindAnyObjectByType(typeof(PlayerInput));
        }

        /// <inheritdoc/>
        protected override void InitializeService()
        {
            Actions ??= new();

            Enable();

            ServiceInitializer.HardInitialize<IInputManager>(this, dontDestroyOnLoad);

            if (ServiceLocator.GetService<IInputManager>() == this as IInputManager)
                ServiceLocator.Register(Actions);
        }


        /// <inheritdoc/>
        public virtual void SetSchemeByDevice(InputDevice device)
        {
            // This had to be added because it was possible for this gameobject to be destroyed and still call this function.
            if (ToString() == NullName)
                return;

            if (device != null)
                EvaluateSchemeByDevice(device);
            else
                UnityEngine.Debug.Log($"REPORT: {ToString()}: Device was Null");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="device"></param>
        protected virtual void EvaluateSchemeByDevice(InputDevice device)
        {
            if (debugInputControls)
                UnityEngine.Debug.Log($"{ToString()}: Device is: {device.name} ");

            var scheme = InputControlScheme.FindControlSchemeForDevice(device, Actions.controlSchemes);

            if (currentScheme != gamepadScheme && device.name == "Core (Plus) Wired Controller")
                SetDeviceScheme(gamepadScheme);

            if (scheme != null && currentScheme == scheme.Value.name)
                return;

            if (scheme != null)
                SetDeviceScheme(scheme.Value.name);
            else if (device.name == MouseName && currentScheme != keyboardScheme)
                SetDeviceScheme(keyboardScheme);
            else if (device.name.Contains(SwitchName) && currentScheme != gamepadScheme)
                SetDeviceScheme(gamepadScheme);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scheme"></param>
        protected virtual void SetDeviceScheme(string scheme)
        {
            if (debugInputControls)
                UnityEngine.Debug.Log($"{ToString()}: Setting Scheme to: " + scheme);

            currentScheme = scheme;

            try
            {
                var bindingGroup = Actions.controlSchemes.First(x => x.name == scheme).bindingGroup;

                Actions.bindingMask = InputBinding.MaskByGroup(bindingGroup);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning("ALERT: Could not set scheme to: " + scheme);

                UnityEngine.Debug.LogWarning("ANALYSIS: " + e.ToString());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="device"></param>
        /// <param name="deviceChange"></param>
        protected virtual void InputSystemOnDeviceChange(InputDevice device, InputDeviceChange deviceChange)
        {
            switch (deviceChange)
            {
                case InputDeviceChange.Added:
                    UnityEngine.Debug.Log("A new device has been added! Device is: " + device.displayName);

                    //TODO: Change pairing from Keyboard to Xbox and dont allow/disable any other schemes
                    //var bindingGroup = playerInputActions.controlSchemes.First(x => x.name == "Switch").bindingGroup;
                    //playerInputActions.bindingMask = InputBinding.MaskByGroup(bindingGroup);

                    SetSchemeByDevice(device);

                    break;

                case InputDeviceChange.Removed:

                    UnityEngine.Debug.Log(device.displayName + "has been removed!");

                    break;

                case InputDeviceChange.Disconnected:

                    UnityEngine.Debug.Log(device.displayName + " has been disconnected!");

                    SetDeviceScheme(keyboardScheme);

                    break;

                case InputDeviceChange.Reconnected:
                    break;
                case InputDeviceChange.Enabled:
                    break;
                case InputDeviceChange.Disabled:
                    break;
                case InputDeviceChange.UsageChanged:
                    break;
                case InputDeviceChange.ConfigurationChanged:
                    break;
            }
        }


        /// <inheritdoc/>
        public virtual string GetCurrentScheme() => currentScheme;


        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (ServiceLocator.HasService<IInputManager>(out var input) && input == this as IInputManager)
                ServiceLocator.Unregister(Actions);
        }



        /// <inheritdoc/>
        public void Enable()
        {
            if (BlockedUpdatingActiveTo(true))
                return;

            InputSystem.onDeviceChange += InputSystemOnDeviceChange;

            Actions?.Enable();

            controls.Enable();
        }

        /// <inheritdoc/>
        public void Disable()
        {
            if (BlockedUpdatingActiveTo(false))
                return;

            InputSystem.onDeviceChange -= InputSystemOnDeviceChange;

            Actions?.Disable();

            controls.Disable();
        }


        protected virtual bool BlockedUpdatingActiveTo(bool active)
        {
            if (active == this.active)
                return true;
            else
                this.active = active;

            return false;
        }


        /// <inheritdoc/>
        public virtual bool UsingGamepadScheme() => GetCurrentScheme() == gamepadScheme;

        /// <inheritdoc/>
        public virtual InputAction GetLeave() => leave.action;

        /// <inheritdoc/>
        public virtual InputAction GetCancel() => cancel.action;
    }
}