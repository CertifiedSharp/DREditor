//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine.InputSystem;

namespace DREditor.Input
{
    /// <summary>
    ///
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        ///
        /// </summary>
        void Enable();

        /// <summary>
        ///
        /// </summary>
        void Disable();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool UsingGamepadScheme();

        /// <summary>
        ///
        /// </summary>
        /// <param name="device"></param>
        void SetSchemeByDevice(InputDevice device);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        string GetCurrentScheme();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        InputAction GetLeave();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        InputAction GetCancel();
    }
}