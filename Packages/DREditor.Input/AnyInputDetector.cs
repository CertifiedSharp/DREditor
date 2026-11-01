//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace DREditor.Input
{
    /// <summary>
    ///
    /// </summary>
    public abstract class AnyInputDetector : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected InputEventPtr eventPtr;

        /// <summary>
        ///
        /// </summary>
        protected InputDevice device;

        /// <summary>
        ///
        /// </summary>
        protected ButtonControl currentControl;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Subscribe() => InputSystem.onEvent += OnAnyInputNoticed;

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Unsubscribe() => InputSystem.onEvent -= OnAnyInputNoticed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventPtr"></param>
        /// <param name="device"></param>
        protected virtual void OnAnyInputNoticed(InputEventPtr eventPtr, InputDevice device)
        {
            InitializeInputData(eventPtr, device);

            CheckNoticedKey();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventPtr"></param>
        /// <param name="device"></param>
        protected virtual void InitializeInputData(InputEventPtr eventPtr, InputDevice device)
        {
            this.eventPtr = eventPtr;

            this.device = device;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckNoticedKey()
        {
            if (ValidKeyNoticed())
                OnInputsDetected();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidKeyNoticed() => eventPtr.IsA<StateEvent>() || eventPtr.IsA<DeltaStateEvent>();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnInputsDetected()
        {
            for (var i = 0; i < device.allControls.Count; ++i)
                if (AssignCurrentControlInvalid(device.allControls[i], out currentControl))
                    continue;
                else if (FoundValidInput())
                    break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool FoundValidInput()
        {
            bool result = CurrentControlIsValid();

            if (result)
                OnValidInputFound();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CurrentControlIsValid() =>
            currentControl.ReadValueFromEvent(eventPtr, out var value) && value >= 1;

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnValidInputFound();

        /// <summary>
        ///
        /// </summary>
        /// <param name="control"></param>
        /// <param name="buttonControl"></param>
        /// <returns></returns>
        protected virtual bool AssignCurrentControlInvalid(InputControl control, out ButtonControl buttonControl)
        {
            buttonControl = control as ButtonControl;

            return buttonControl == null || buttonControl.synthetic || buttonControl.noisy;
        }
    }
}