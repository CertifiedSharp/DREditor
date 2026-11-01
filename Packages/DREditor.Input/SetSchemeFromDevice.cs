//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace DREditor.Input
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AnyInputDetector) +
        "\n\n")]
    public class SetSchemeFromDevice : AnyInputDetector
    {
        /// <summary>
        ///
        /// </summary>
        protected InputDevice latestDevice = null;

        /// <summary>
        ///
        /// </summary>
        protected IInputManager inputManager = null;

        /// <summary>
        ///
        /// </summary>
        protected readonly Dictionary<string, List<string>> schemeDevices = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => Subscribe();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => Unsubscribe();


        /// <summary>
        ///
        /// </summary>
        protected override void OnValidInputFound()
        {
            if (SchemeShouldChange())
                ChangeScheme();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SchemeShouldChange() => 
            InputManagerValid() && DeviceIsValid() && !DeviceEntryAlreadyRegistered();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InputManagerValid() => ServiceLocator.HasService(out inputManager);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DeviceIsValid() => device != null && device != latestDevice;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DeviceEntryAlreadyRegistered() => SchemeRegistered() && RegisteredSchemeMatchesDevice();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SchemeRegistered() => schemeDevices.ContainsKey(inputManager.GetCurrentScheme());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool RegisteredSchemeMatchesDevice() =>
            schemeDevices[inputManager.GetCurrentScheme()].Contains(device.name);


        /// <summary>
        ///
        /// </summary>
        protected virtual void ChangeScheme()
        {
            LogUpdatedDevice();

            UpdateSchemeUsingDevice();

            UpdateRegistryWithLatest();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LogUpdatedDevice()
        {
            string log = latestDevice != null ?
                device.name + " from: " + latestDevice.name :
                device.name;

            UnityEngine.Debug.Log($"{ToString()}: Input Detect Update: " + log);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateSchemeUsingDevice()
        {
            latestDevice = device;

            inputManager.SetSchemeByDevice(latestDevice);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateRegistryWithLatest()
        {
            if (SchemeRegistered())
                UpdateScheme();
            else
                RegisterScheme();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateScheme() => schemeDevices[inputManager.GetCurrentScheme()].Add(device.name);

        /// <summary>
        ///
        /// </summary>
        protected virtual void RegisterScheme() =>
            schemeDevices.Add(inputManager.GetCurrentScheme(), new List<string>() { device.name });
    }
}