//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader( nameof(CallableUIBase) +
        "\n\n")]
    public class CallableControlledUI : CallableUIBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;


        protected virtual void OnEnable() => inputAllowed.AddListener(OnInputUpdated);

        protected virtual void OnDisable() => inputAllowed.RemoveListener(OnInputUpdated);

        /// <inheritdoc/>
        protected override void CallControls(InputAction.CallbackContext context)
        {
            if (!inputAllowed.Value)
                return;

            base.CallControls(context);
        }

        /// <inheritdoc/>
        protected override void OpenControlsMenu()
        {
            OnUIOpened();

            base.OpenControlsMenu();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnUIOpened() => inputAllowed.SetValue(false);

        /// <inheritdoc/>
        protected override IEnumerator WaitForControlsEnd()
        {
            yield return base.WaitForControlsEnd();

            OnUIClosed();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnUIClosed() => inputAllowed.SetValue(true);
    }
}